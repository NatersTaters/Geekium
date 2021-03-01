using Geekium.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Geekium.Controllers
{
    public class MyListingsController : Controller
    {

        private readonly GeekiumContext _context;

        #region Trade
        // GET: TradeListings/Create
        public IActionResult CreateTrade()
        {
            ViewData["SellerId"] = new SelectList(_context.SellerAccounts, "SellerId", "SellerId");
            return View();
        }

        // POST: TradeListings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTrade([Bind("TradeListingId,SellerId,TradeTitle,TradeDescription,TradeFor,TradeDate,TradeItemType,TradeQuantity")] TradeListing tradeListing)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tradeListing);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SellerId"] = new SelectList(_context.SellerAccounts, "SellerId", "SellerId", tradeListing.SellerId);
            return View(tradeListing);
        }
        #endregion

        #region Selling
        // GET: SellListings/Create
        public IActionResult CreateSell()
        {
            ViewData["PriceTrendId"] = new SelectList(_context.PriceTrends, "PriceTrendId", "PriceTrendId");
            ViewData["SellerId"] = new SelectList(_context.SellerAccounts, "SellerId", "SellerId");
            return View();
        }

        // POST: SellListings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSell([Bind("SellListingId,SellerId,PriceTrendId,SellTitle,SellDescription,SellPrice,SellDate,SellItemType,SellQuantity")] SellListing sellListing)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sellListing);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PriceTrendId"] = new SelectList(_context.PriceTrends, "PriceTrendId", "PriceTrendId", sellListing.PriceTrendId);
            ViewData["SellerId"] = new SelectList(_context.SellerAccounts, "SellerId", "SellerId", sellListing.SellerId);
            return View(sellListing);
        }
        #endregion

        #region Service
        // GET: ServiceListings/Create
        public IActionResult CreateService()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "Email");
            return View();
        }

        // POST: ServiceListings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateService([Bind("ServiceListingId,AccountId,ServiceTitle,ServiceDescription,ListingDate")] ServiceListing serviceListing)
        {
            if (ModelState.IsValid)
            {
                _context.Add(serviceListing);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "Email", serviceListing.AccountId);
            return View(serviceListing);
        }
        #endregion


        public MyListingsController(GeekiumContext context)
        {
            _context = context;
        }

        // GET: SellListings
        public async Task<IActionResult> Index()
        {
            var geekiumContext = _context.SellListings.Include(s => s.PriceTrend).Include(s => s.Seller);
            return View(await geekiumContext.ToListAsync());
        }
    }
}
