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
    public class TradeListingsController : Controller
    {
        private readonly GeekiumContext _context;

        public TradeListingsController(GeekiumContext context)
        {
            _context = context;
        }

        // GET: TradeListings
        public async Task<IActionResult> Index()
        {
            var geekiumContext = _context.TradeListings.Include(t => t.Seller).Include(t => t.Seller.Account);
            SetViewBag(null);
            return View(await geekiumContext.ToListAsync());
        }

        // GET: TradeListings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tradeListing = await _context.TradeListings
                .Include(t => t.Seller)
                .FirstOrDefaultAsync(m => m.TradeListingId == id);
            if (tradeListing == null)
            {
                return NotFound();
            }

            return View(tradeListing);
        }

        // When the user tries to search for an item, we will query it 
        [HttpPost]
        public async Task<IActionResult> FilterTrades(string searchTrade)
        {
            if (searchTrade != null && searchTrade != "")
            {
                var geekiumContext = _context.TradeListings.Include(t => t.Seller).Include(t => t.Seller.Account)
                    .Where(s => s.TradeTitle.ToLower().Contains(searchTrade.ToLower()));
                SetViewBag(searchTrade);
                return View("Index", await geekiumContext.ToListAsync());

            }
            else
            {
                SetViewBag(null);
                return RedirectToAction("Index");
            }
        }

        void SetViewBag(string search)
        {
            ViewBag.Collapse = "collapse";

            if (search != null && search != "")
                ViewBag.Search = search;
            else
                ViewBag.Search = null;
        }
    }
}
