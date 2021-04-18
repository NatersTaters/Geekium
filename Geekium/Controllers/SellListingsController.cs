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
            var geekiumContext = _context.SellListings.Include(s => s.PriceTrend)
                .Include(s => s.Seller)
                .Include(s => s.Seller.Account)
                .Where(s => s.Seller.AccountId != 1)
                .Where(s => s.Display == true);

            List<SelectListItem> dropdownList = PopulateDropdown(null);
            ViewBag.SellFilter = dropdownList;

            SetViewBag(null, 0, 0, null);
            var model = await geekiumContext.ToListAsync();
            return View(model);
        }

        // Display the merchandise sell listings
        public async Task<IActionResult> MerchandiseIndex()
        {
            // Only display the sell listings of the administrator (accountId: 2 is admin)
            var geekiumContext = _context.SellListings.Include(s => s.PriceTrend)
                .Include(s => s.Seller)
                .Include(s => s.Seller.Account)
                .Where(s => s.Seller.AccountId == 1)
                .Where(s => s.Display == true);

            List<SelectListItem> dropdownList = PopulateDropdown(null);
            ViewBag.MerchandiseFilter = dropdownList;

            SetViewBag(null, 0, 0, null);
            var model = await geekiumContext.ToListAsync();
            return View(model);
        }

        // When they hit back, go to the correct index
        public async Task<IActionResult> ChooseIndex(int id)
        {
            var isThisMerch = await _context.SellListings.Include(s => s.PriceTrend)
                .Include(s => s.Seller)
                .Include(s => s.Seller.Account)
                .FirstOrDefaultAsync(s => s.SellListingId == id);

            try
            {
                if (isThisMerch.Seller.AccountId == 2)
                    return RedirectToAction("MerchandiseIndex");
                else
                    return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }

        // Filter products with any search/filter parameters available
        [HttpPost]
        public async Task<IActionResult> FilterProducts(string searchProduct, float minPrice, float maxPrice)
        {
            string type = "";
            try
            {
                type = Request.Form["typeList"].ToString();
            }
            catch (Exception)
            {
                type = "";
            }

            var geekiumContext = _context.SellListings
                .Include(s => s.PriceTrend)
                .Include(s => s.Seller)
                .Include(s => s.Seller.Account)
                .Where(s => s.Seller.AccountId != 2)
                .Where(s => s.Display == true);

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


            var filterContext = geekiumContext;
            if (searchProduct != null && searchProduct != "")
                filterContext = geekiumContext.Where(s => s.SellTitle.Contains(searchProduct));

            var priceContext = filterContext;
            if (minPrice >= 0 && maxPrice > 0)
                priceContext = filterContext.Where(s => s.SellPrice >= minPrice && s.SellPrice <= maxPrice);

            var typeContext = priceContext;
            if (type != "")
                typeContext = priceContext.Where(s => s.SellItemType == type);

            List<SelectListItem> dropdownList = PopulateDropdown(type);
            ViewBag.SellFilter = dropdownList;

            SetViewBag(searchProduct, minPrice, maxPrice, type);
            return View("Index", await typeContext.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> FilterMerchandise(string searchProduct, float minPrice, float maxPrice)
        {
            string type = "";
            try
            {
                type = Request.Form["typeList"].ToString();
            }
            catch (Exception)
            {
                type = "";
            }

            var geekiumContext = _context.SellListings
                .Include(s => s.PriceTrend)
                .Include(s => s.Seller)
                .Include(s => s.Seller.Account)
                .Where(s => s.Seller.AccountId == 2)
                .Where(s => s.Display == true);

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


            var filterContext = geekiumContext;
            if (searchProduct != null && searchProduct != "")
                filterContext = geekiumContext.Where(s => s.SellTitle.Contains(searchProduct));

            var priceContext = filterContext;
            if (minPrice >= 0 && maxPrice > 0)
                priceContext = filterContext.Where(s => s.SellPrice >= minPrice && s.SellPrice <= maxPrice);

            var typeContext = priceContext;
            if (type != "")
                typeContext = priceContext.Where(s => s.SellItemType == type);

            List<SelectListItem> dropdownList = PopulateDropdown(type);
            ViewBag.MerchandiseFilter = dropdownList;

            SetViewBag(searchProduct, minPrice, maxPrice, type);
            return View("MerchandiseIndex", await typeContext.ToListAsync());
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
        public void SetViewBag(string search, float min, float max, string type)
        {
            if (search != null && search != "")
                ViewBag.Search = search;
            else
                ViewBag.Search = null;

            if (min == 0 && max == 0 && (type == "" || type == null))
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

        public List<SelectListItem> PopulateDropdown(string type)
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
