using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uzdevums.Data;
using Uzdevums.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;


namespace Uzdevums.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationContext _dbcontext;
        
        public AccountController(ApplicationContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModule)
        {
            if (ModelState.IsValid)
            {
                User user = _dbcontext.Users.FirstOrDefault(u => u.Name.ToLower() == loginModule.Name.ToLower());
                
                if (user is null)
                {
                    ModelState.AddModelError("Name", "Tāds lietotājs neeksistē");
                }
                else 
                if (user.Password != loginModule.Password)
                {
                    ModelState.AddModelError("Password", "Nepareiza parole");
                }
                else
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
                        new Claim("id", user.Id.ToString())
                    };

                    if (user.IsAdmin)
                    {
                        claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, "admin"));
                    };

                    ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                    
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));

                    return Redirect("/");
                }

                return View(loginModule);
            }
            else
            {
                ModelState.AddModelError("", "Nav ievadīts lietotājs vai parole");
                return View(loginModule);
            }
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
