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
    public class CartsController : Controller
    {
        private readonly GeekiumContext _context;

        public CartsController(GeekiumContext context)
        {
            _context = context;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            var geekiumContext = _context.Cart.Include(c => c.Account);
            return View(await geekiumContext.ToListAsync());
        }

      

        private bool CartExists(int id)
        {
            return _context.Cart.Any(e => e.CartId == id);
        }

        //Checkout - THIS IS WHERE PAYMENT PORTAL
        //public async Task<IActionResult> CheckOut()
        public IActionResult CheckOut()
        {
            return View();
        }
    }
}
