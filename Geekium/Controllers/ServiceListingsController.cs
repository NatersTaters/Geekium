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


        // GET: ServiceListings/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

        // GET: ServiceListings/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: ServiceListings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceListing = await _context.ServiceListings.FindAsync(id);
            _context.ServiceListings.Remove(serviceListing);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceListingExists(int id)
        {
            return _context.ServiceListings.Any(e => e.ServiceListingId == id);
        }
    }
}
