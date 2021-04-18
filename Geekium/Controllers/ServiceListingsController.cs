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
    public class ServiceListingsController : Controller
    {
        private readonly GeekiumContext _context;

        public ServiceListingsController(GeekiumContext context)
        {
            _context = context;
        }

        // Display all service listings
        public async Task<IActionResult> Index()
        {
            var geekiumContext = _context.ServiceListings
                .Include(s => s.Account);

            List<SelectListItem> dropdownList = PopulateDropdown(null);
            ViewBag.ServiceFilter = dropdownList;

            SetViewBag(null, null);
            return View(await geekiumContext.ToListAsync());
        }

        // Get the details of the passed in service id
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceListing = await _context.ServiceListings
                .Include(s => s.Account)
                .FirstOrDefaultAsync(m => m.ServiceListingId == id);
            if (serviceListing == null)
            {
                return NotFound();
            }

            return View(serviceListing);
        }

        // Filter the service listings based on filter and search parameters
        [HttpPost]
        public async Task<IActionResult> FilterServices(string searchService)
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
            var geekiumContext = _context.ServiceListings
                .Include(s => s.Account);

            var filterContext = (IQueryable<ServiceListing>)geekiumContext;
            if (searchService != null && searchService != "")
                filterContext = geekiumContext.Where(s => s.ServiceTitle.Contains(searchService));


            var typeContext = filterContext;
            if (type != "")
                typeContext = filterContext.Where(s => s.ServiceLocation == type);

            List<SelectListItem> dropdownList = PopulateDropdown(type);
            ViewBag.ServiceFilter = dropdownList;
            SetViewBag(searchService, type);

            return View("Index", await typeContext.ToListAsync());
        }

        // Set view bag based on filter
        public void SetViewBag(string search, string type)
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

        public List<SelectListItem> PopulateDropdown(string type)
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
