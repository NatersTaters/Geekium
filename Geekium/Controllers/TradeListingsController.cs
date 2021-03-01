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

        // GET: TradeListings
        public async Task<IActionResult> Index()
        {
            var geekiumContext = _context.TradeListings.Include(t => t.Seller);
            return View(await geekiumContext.ToListAsync());
        }

        // GET: TradeListings/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: TradeListings/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

        // GET: TradeListings/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: TradeListings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tradeListing = await _context.TradeListings.FindAsync(id);
            _context.TradeListings.Remove(tradeListing);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TradeListingExists(int id)
        {
            return _context.TradeListings.Any(e => e.TradeListingId == id);
        }
    }
}
