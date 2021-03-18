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
    public class SellListingsController : Controller
    {
        private readonly GeekiumContext _context;

        public SellListingsController(GeekiumContext context)
        {
            _context = context;
        }

        // GET: SellListings
        public async Task<IActionResult> Index()
        {
            var geekiumContext = _context.SellListings.Include(s => s.PriceTrend).Include(s => s.Seller).Include(s => s.Seller.Account);
            SetViewBag(null, 0, 0);
            var model = await geekiumContext.ToListAsync();
            return View(model);
        }

        // When they hit the "edit listings"
        // They will be redirected to a page with only their own listings
        public async Task<IActionResult> UserSellListings()
        {
            var geekiumContext = _context.SellListings.Include(s => s.PriceTrend).Include(s => s.Seller);
            return View(await geekiumContext.ToListAsync());
        }

        // When the user tries to search for an item, we will query it 
        [HttpPost]
        public async Task<IActionResult> FilterProducts(string searchProduct, float minPrice, float maxPrice)
        {
            if (minPrice > maxPrice)
            {
                float holdMinimum = maxPrice;
                maxPrice = minPrice;
                minPrice = holdMinimum;
            }

            if (searchProduct != null && searchProduct != "")
            {
                if (minPrice != 0 && maxPrice != 0)
                {
                    var geekiumContext = _context.SellListings.Include(s => s.PriceTrend).Include(s => s.Seller)
                        .Where(s => s.SellPrice >= minPrice && s.SellPrice <= maxPrice && s.SellTitle.Contains(searchProduct));
                    SetViewBag(searchProduct, minPrice, maxPrice);
                    return View("Index", await geekiumContext.ToListAsync());
                }
                else
                {
                    var geekiumContext = _context.SellListings.Include(s => s.PriceTrend).Include(s => s.Seller)
                        .Where(s => s.SellTitle.Contains(searchProduct));
                    SetViewBag(searchProduct, 0, 0);
                    return View("Index", await geekiumContext.ToListAsync());
                }
            }
            else if (minPrice != 0 && maxPrice != 0)
            {
                var geekiumContext = _context.SellListings.Include(s => s.PriceTrend).Include(s => s.Seller)
                    .Where(s => s.SellPrice >= minPrice && s.SellPrice <= maxPrice);
                SetViewBag(null, minPrice, maxPrice);
                return View("Index", await geekiumContext.ToListAsync());
            }
            else
            {
                SetViewBag(null, 0, 0);
                return RedirectToAction("Index");
            }


        }

        // When the user clicks on any item, the details will be returned
        public async Task<IActionResult> Details(int ? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sellListing = await _context.SellListings
                .Include(s => s.PriceTrend)
                .Include(s => s.Seller)
                .FirstOrDefaultAsync(m => m.SellListingId == id);
            if (sellListing == null)
            {
                return NotFound();
            }

            return View(sellListing);
        }
        void SetViewBag(string search, float min, float max)
        {
            if (search != null && search != "")
                ViewBag.Search = search;
            else
                ViewBag.Search = null;

            if (min == 0 && max == 0)
            {
                ViewBag.Collapse = "collapse";
                ViewBag.MinPrice = null;
                ViewBag.MaxPrice = null;
            }
            else
            {
                ViewBag.Collapse = "collapse in show";
                ViewBag.MinPrice = min;
                ViewBag.MaxPrice = max;
            }
        }
    }
}
