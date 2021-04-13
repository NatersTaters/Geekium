using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Geekium.Models;
using Microsoft.AspNetCore.Http;

namespace Geekium.Controllers
{
    public class SellListingsController : Controller
    {
        private readonly GeekiumContext _context;

        public SellListingsController(GeekiumContext context)
        {
            _context = context;
        }

        // Display the current sell listings
        public async Task<IActionResult> Index()
        {
            string userId = HttpContext.Session.GetString("userId");
            DateTime defaultDate = DateTime.Now;

            var geekiumContext = _context.SellListings.Include(s => s.PriceTrend)
                .Include(s => s.Seller)
                .Include(s => s.Seller.Account)
                .Where(s => s.Seller.AccountId != 2);

            List<SelectListItem> dropdownList = PopulateDropdown(null);
            ViewBag.SellFilter = dropdownList;

            SetViewBag(null, 0, 0, 0, 0, defaultDate, defaultDate);
            var model = await geekiumContext.ToListAsync();
            return View(model);
        }

        // Display the merchandise sell listings
        public async Task<IActionResult> MerchandiseIndex()
        {
            // Only display the sell listings of the administrator (accountId: 2 is admin)
            string userId = HttpContext.Session.GetString("userId");
            DateTime defaultDate = new DateTime();

            var geekiumContext = _context.SellListings.Include(s => s.PriceTrend)
                .Include(s => s.Seller)
                .Include(s => s.Seller.Account)
                .Where(s => s.Seller.AccountId == 2);

            SetViewBag(null, 0, 0, 0, 0, defaultDate, defaultDate);
            var model = await geekiumContext.ToListAsync();
            return View(model);
        }

        // When they hit back, go to the correct index
        public async Task<IActionResult> ChooseIndex(int id)
        {
            string userId = HttpContext.Session.GetString("userId");
            var isThisMerch = await _context.SellListings.Include(s => s.PriceTrend)
                .Include(s => s.Seller)
                .Include(s => s.Seller.Account)
                .FirstOrDefaultAsync(s => s.SellListingId == id);

            if (isThisMerch.Seller.AccountId == 2)
                return RedirectToAction("MerchandiseIndex");
            else
                return RedirectToAction("Index");
        }

        // Filter products with any search/filter parameters available
        [HttpPost]
        public async Task<IActionResult> FilterProducts(string searchProduct, float minPrice, float maxPrice, 
            int minQuantity, int maxQuantity, DateTime fromDate, DateTime toDate)
        {
            string type = Request.Form["typeList"].ToString();
            var geekiumContext = _context.SellListings
                .Include(s => s.PriceTrend)
                .Include(s => s.Seller)
                .Include(s => s.Seller.Account)
                .Where(s => s.Seller.AccountId != 2);

            if (minPrice > maxPrice)
            {
                float holdMinimum = maxPrice;
                maxPrice = minPrice;
                minPrice = holdMinimum;
            }

            if (minPrice < 0)
                minPrice = 0;

            if (maxPrice < 0)
                maxPrice = 0;

            if (minQuantity < 0)
                minQuantity = 0;

            if (maxQuantity < 0)
                maxQuantity = 0;

            var filterContext = geekiumContext;
            if (searchProduct != null && searchProduct != "")
                filterContext = geekiumContext.Where(s => s.SellTitle.Contains(searchProduct));

            var priceContext = filterContext;
            if (minPrice >= 0 && maxPrice > 0)
                priceContext = filterContext.Where(s => s.SellPrice >= minPrice && s.SellPrice <= maxPrice);

            var typeContext = priceContext;
            if (type != "")
                typeContext = priceContext.Where(s => s.SellItemType == type);

            var quantityContext = typeContext;
            if (minQuantity >= 0 && maxQuantity > 0)
                quantityContext = typeContext.Where(s => s.SellQuantity >= minQuantity && s.SellQuantity <= maxQuantity);

            var dateContext = quantityContext;
            if (fromDate != new DateTime() && toDate != new DateTime())
                dateContext = quantityContext.Where(s => s.SellDate >= fromDate && s.SellDate <= toDate);

            List<SelectListItem> dropdownList = PopulateDropdown(type);
            ViewBag.SellFilter = dropdownList;

            SetViewBag(searchProduct, minPrice, maxPrice, minQuantity, maxQuantity, fromDate, toDate);
            return View("Index", await dateContext.ToListAsync());
        }

        // When the user clicks on any item, the details will be returned
        public async Task<IActionResult> Details(int ? id)
        {
            if (id == null)
                return NotFound();

            var sellListing = await _context.SellListings
                .Include(s => s.PriceTrend)
                .Include(s => s.Seller)
                .Include(s => s.Seller.Account)
                .FirstOrDefaultAsync(m => m.SellListingId == id);

            if (sellListing == null)
                return NotFound();

            string userId = HttpContext.Session.GetString("userId");
            if (sellListing.Seller.AccountId.ToString() == userId)
                ViewBag.ShowCart = false;
            else
                ViewBag.ShowCart = true;
                    
            return View(sellListing);
        }

        // Sets view bag based on filter
        void SetViewBag(string search, float min, float max, int minQuantity, int maxQuantity,
            DateTime fromDate, DateTime toDate)
        {
            string defaultDate = DateTime.Now.ToString("yyyy-MM-dd");

            if (search != null && search != "")
                ViewBag.Search = search;
            else
                ViewBag.Search = null;

            if (min == 0 && max == 0 && minQuantity == 0 && maxQuantity == 0 && 
                fromDate.Date.ToString("yyyy-MM-dd") == defaultDate && toDate.Date.ToString("yyyy-MM-dd") == defaultDate)
            {
                ViewBag.Collapse = "collapse";
                ViewBag.MinPrice = null;
                ViewBag.MaxPrice = null;
                ViewBag.MinQuantity = null;
                ViewBag.MaxQuantity = null;
                ViewBag.FromDate = DateTime.Now;
                ViewBag.ToDate = DateTime.Now;
            }
            else
            {
                ViewBag.Collapse = "collapse in show";
                ViewBag.MinPrice = min;
                ViewBag.MaxPrice = max;
                ViewBag.MinQuantity = minQuantity;
                ViewBag.MaxQuantity = maxQuantity;

                ViewBag.FromDate = fromDate.Date;
                ViewBag.ToDate = toDate.Date;
            }
        }

        private List<SelectListItem> PopulateDropdown(string type)
        {
            List<SelectListItem> drop = new List<SelectListItem>();

            foreach (var item in DropDownValues.dropdownValues)
            {
                SelectListItem select = new SelectListItem
                {
                    Selected = false,
                    Text = item,
                    Value = item
                };
                drop.Add(select);
            }

            foreach (var item in drop)
            {
                if (item.Value == type)
                {
                    item.Selected = true;
                    break;
                }
            }

            return drop;
        }
    }
}
