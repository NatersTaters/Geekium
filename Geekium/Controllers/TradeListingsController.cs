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
            var geekiumContext = _context.TradeListings
                .Include(t => t.Seller)
                .Include(t => t.Seller.Account);

            List<SelectListItem> dropdownList = PopulateDropdown(null);
            ViewBag.TradeFilter = dropdownList;

            SetViewBag(null, null);
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
            string type = Request.Form["typeList"].ToString();
            var geekiumContext = _context.TradeListings
                .Include(s => s.Seller.Account);

            var filterContext = (IQueryable<TradeListing>)geekiumContext;
            if (searchTrade != null && searchTrade != "")
                filterContext = geekiumContext.Where(s => s.TradeTitle.Contains(searchTrade));


            var typeContext = filterContext;
            if (type != "")
                typeContext = filterContext.Where(s => s.TradeLocation == type);

            List<SelectListItem> dropdownList = PopulateDropdown(type);
            ViewBag.TradeFilter = dropdownList;
            SetViewBag(searchTrade, type);

            return View("Index", await typeContext.ToListAsync());
        }

        // Set view bag based on filter
        void SetViewBag(string search, string type)
        {
            ViewBag.Collapse = "collapse";

            if (search != null && search != "")
                ViewBag.Search = search;
            else
                ViewBag.Search = null;

            if (type == "" || type == null)
                ViewBag.Collapse = "collapse";
            else
                ViewBag.Collapse = "collapse in show";
        }
        private List<SelectListItem> PopulateDropdown(string type)
        {
            List<SelectListItem> drop = new List<SelectListItem>();

            foreach (var item in DropDownValues.provinceDictionary)
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
