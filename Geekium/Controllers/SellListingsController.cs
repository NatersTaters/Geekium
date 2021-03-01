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
            var geekiumContext = _context.SellListings.Include(s => s.PriceTrend).Include(s => s.Seller);
            return View(await geekiumContext.ToListAsync());
        }

        // GET: SellListings/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: SellListings/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

        // GET: SellListings/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: SellListings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sellListing = await _context.SellListings.FindAsync(id);
            _context.SellListings.Remove(sellListing);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SellListingExists(int id)
        {
            return _context.SellListings.Any(e => e.SellListingId == id);
        }


        // When they hit the "edit listings"
        // They will be redirected to a page with only their own listings
        public async Task<IActionResult> UserSellListings()
        {
            var geekiumContext = _context.SellListings.Include(s => s.PriceTrend).Include(s => s.Seller);
            return View(await geekiumContext.ToListAsync());
        }
    }
}
