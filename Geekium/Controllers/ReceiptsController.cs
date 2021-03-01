using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Geekium.Models;

namespace Geekium.Controllers
{
    public class ReceiptsController : Controller
    {
        private readonly GeekiumContext _context;

        public ReceiptsController(GeekiumContext context)
        {
            _context = context;
        }

        // GET: Receipts
        public async Task<IActionResult> Index()
        {
            var geekiumContext = _context.Receipt.Include(r => r.Cart);
            return View(await geekiumContext.ToListAsync());
        }
    }
}
