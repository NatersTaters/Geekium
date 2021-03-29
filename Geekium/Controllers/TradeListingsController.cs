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

        // Get all trade listings
        public async Task<IActionResult> Index()
        {
            var geekiumContext = _context.TradeListings.Include(t => t.Seller).Include(t => t.Seller.Account);
            SetViewBag(null);
            return View(await geekiumContext.ToListAsync());
        }

        // Display the details page for the given trade id
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tradeListing = await _context.TradeListings
                .Include(t => t.Seller)
                .Include(t => t.Seller.Account)
                .FirstOrDefaultAsync(m => m.TradeListingId == id);
            if (tradeListing == null)
            {
                return NotFound();
            }

            return View(tradeListing);
        }

        // Filter the listings page based on filter and search parameters
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

        // Set view bag based on filter
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
