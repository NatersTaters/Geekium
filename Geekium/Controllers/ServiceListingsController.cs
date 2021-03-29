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

        // GET: ServiceListings
        public async Task<IActionResult> Index()
        {
            var geekiumContext = _context.ServiceListings.Include(s => s.Account);
            SetViewBag(null);
            return View(await geekiumContext.ToListAsync());
        }

        // GET: ServiceListings/Details/5
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

        [HttpPost]
        public async Task<IActionResult> FilterServices(string searchService)
        {
            if (searchService != null && searchService != "")
            {
                var geekiumContext = _context.ServiceListings.Include(t => t.Account);
                SetViewBag(searchService);
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
