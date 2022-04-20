using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Uzdevums.Models;
using Microsoft.AspNetCore.Authorization;
using Uzdevums.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Uzdevums.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationContext _context;

        private readonly ILogger<ProductController> _logger;

        private IConfiguration Configuration { get; }

        public ProductController(ILogger<ProductController> logger, ApplicationContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            Configuration = configuration;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            int _pvn = Configuration.GetValue<int>("PVN");
            ViewBag.PVN = _pvn;
            return View(await _context.Products.ToListAsync());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public async Task<IActionResult> Detail(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product is null)
            {
                return NotFound();
            }
            else
                return View(product);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {            
                _context.Add(product);
                await _context.SaveChangesAsync();
                LogChanges(product.Id, "created");
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product is null)
            {
                return NotFound();
            }
            return View(product);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,NumberOfUnits,PricePerUnit")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    _context.Update(product);
                    LogChanges(product.Id, "edit");
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);

            if (product is null)
            {
                return NotFound();
            }

            return View(product);
        }

        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            LogChanges(product.Id, "deleted");
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ProductExists(int id)
        {
            return await _context.Products.AnyAsync(e => e.Id == id);
        }

        public void LogChanges(int productId, string changeText)
        {
            ChangeLog changelog = new ChangeLog();
            changelog.CreatedOn = DateTime.Now;
            changelog.ProductId = productId;
            string tmpid = User.FindFirst(x => x.Type == "id").Value;
            changelog.UserUserId = int.Parse(tmpid);
            changelog.ChangeText = changeText;
            _context.ChangeLogs.Add(changelog);
        }
    }
}
