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

        #region Create
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
        #region Details
        public async Task<IActionResult> TradeDetails(int? id)
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
        #endregion
        #region Delete
        // POST: TradeListings/Delete/5
        [HttpPost, ActionName("TradeDetails")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedTrade(int id)
        {
            var tradeListing = await _context.TradeListings.FindAsync(id);
            _context.TradeListings.Remove(tradeListing);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion
        #region Edit
        // GET: TradeListings/Edit/5
        public async Task<IActionResult> EditTrade(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tradeListing = await _context.TradeListings.FindAsync(id);
            if (tradeListing == null)
            {
                return NotFound();
            }
            ViewData["SellerId"] = new SelectList(_context.SellerAccounts, "SellerId", "SellerId", tradeListing.SellerId);
            return View(tradeListing);
        }

        // POST: TradeListings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TradeListingId,SellerId,TradeTitle,TradeDescription,TradeFor,TradeDate,TradeItemType,TradeQuantity")] TradeListing tradeListing)
        {
            if (id != tradeListing.TradeListingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tradeListing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TradeListingExists(tradeListing.TradeListingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SellerId"] = new SelectList(_context.SellerAccounts, "SellerId", "SellerId", tradeListing.SellerId);
            return View(tradeListing);
        }


        private bool TradeListingExists(int id)
        {
            return _context.TradeListings.Any(e => e.TradeListingId == id);
        }

        #endregion

        #endregion

        #region Selling

        #region Create
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
        #region Details
        public async Task<IActionResult> SellDetails(int? id)
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
        #endregion
        #region Delete
        // POST: SellListings/Delete/5
        [HttpPost, ActionName("SellDetails")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedSelling(int id)
        {
            var sellListing = await _context.SellListings.FindAsync(id);
            _context.SellListings.Remove(sellListing);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion
        #region Edit
        // GET: SellListings/Edit/5
        public async Task<IActionResult> EditSell(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sellListing = await _context.SellListings.FindAsync(id);
            if (sellListing == null)
            {
                return NotFound();
            }
            ViewData["PriceTrendId"] = new SelectList(_context.PriceTrends, "PriceTrendId", "PriceTrendId", sellListing.PriceTrendId);
            ViewData["SellerId"] = new SelectList(_context.SellerAccounts, "SellerId", "SellerId", sellListing.SellerId);
            return View(sellListing);
        }

        // POST: SellListings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SellListingId,SellerId,PriceTrendId,SellTitle,SellDescription,SellPrice,SellDate,SellItemType,SellQuantity")] SellListing sellListing)
        {
            if (id != sellListing.SellListingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sellListing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SellListingExists(sellListing.SellListingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PriceTrendId"] = new SelectList(_context.PriceTrends, "PriceTrendId", "PriceTrendId", sellListing.PriceTrendId);
            ViewData["SellerId"] = new SelectList(_context.SellerAccounts, "SellerId", "SellerId", sellListing.SellerId);
            return View(sellListing);
        }


        private bool SellListingExists(int id)
        {
            return _context.SellListings.Any(e => e.SellListingId == id);
        }
        #endregion


        #endregion

        #region Service

        #region Create
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
        #region Details
        public async Task<IActionResult> ServiceDetails(int? id)
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

        #endregion
        #region Delete
        // POST: ServiceListings/Delete/5
        [HttpPost, ActionName("ServiceDetails")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedService(int id)
        {
            var serviceListing = await _context.ServiceListings.FindAsync(id);
            _context.ServiceListings.Remove(serviceListing);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion
        #region Edit


        // GET: ServiceListings/Edit/5
        public async Task<IActionResult> EditService(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceListing = await _context.ServiceListings.FindAsync(id);
            if (serviceListing == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "Email", serviceListing.AccountId);
            return View(serviceListing);
        }

        // POST: ServiceListings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceListingId,AccountId,ServiceTitle,ServiceDescription,ListingDate")] ServiceListing serviceListing)
        {
            if (id != serviceListing.ServiceListingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceListing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceListingExists(serviceListing.ServiceListingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "Email", serviceListing.AccountId);
            return View(serviceListing);
        }


        private bool ServiceListingExists(int id)
        {
            return _context.ServiceListings.Any(e => e.ServiceListingId == id);
        }

        #endregion

        #endregion

        public MyListingsController(GeekiumContext context)
        {
            _context = context;
        }

        // GET: SellListings
        public async Task<IActionResult> Index()
        {
            var context = _context.SellerAccounts.Include(s => s.SellListings).Include(s => s.TradeListings)
                .Include(s => s.Account.ServiceListings);

            return View(await context.ToListAsync());
        }
    }
}
