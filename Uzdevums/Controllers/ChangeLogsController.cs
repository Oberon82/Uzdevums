using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uzdevums.Data;
using Uzdevums.Models;

namespace Uzdevums.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChangeLogsController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public ChangeLogsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/ChangeLogs
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<ChangeLog>>> GetChangeLogs()
        {
            var s = HttpContext.Request.Path;
            // riež 4. punktā uzkrātos
            // auditācijas ierakstus. Pēc noklusējuma tiek atgriezti 10
            // jaunākie ieraksti un tos var filtrēt pēc datuma(no, līdz)

            // !! Pieņemu, ka filtrēšana notiek klienta galā???
            return await _context.ChangeLogs.OrderByDescending(p => p.Id).Take(10).ToListAsync();
        }
    }
}
