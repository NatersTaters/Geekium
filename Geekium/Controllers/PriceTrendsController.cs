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
    public class PriceTrendsController : Controller
    {
        private readonly GeekiumContext _context;

        public PriceTrendsController(GeekiumContext context)
        {
            _context = context;
        }

        // GET: PriceTrends
        public async Task<IActionResult> Index()
        {
            return View(await _context.PriceTrends.ToListAsync());
        }

        // GET: PriceTrends/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var priceTrend = await _context.PriceTrends
                .FirstOrDefaultAsync(m => m.PriceTrendId == id);
            if (priceTrend == null)
            {
                return NotFound();
            }

            return View(priceTrend);
        }

        // GET: PriceTrends/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PriceTrends/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PriceTrendId,ItemName,DateOfUpdate,AveragePrice,LowestPrice,HighestPrice")] PriceTrend priceTrend)
        {
            if (ModelState.IsValid)
            {
                _context.Add(priceTrend);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(priceTrend);
        }

        // GET: PriceTrends/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var priceTrend = await _context.PriceTrends.FindAsync(id);
            if (priceTrend == null)
            {
                return NotFound();
            }
            return View(priceTrend);
        }

        // POST: PriceTrends/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PriceTrendId,ItemName,DateOfUpdate,AveragePrice,LowestPrice,HighestPrice")] PriceTrend priceTrend)
        {
            if (id != priceTrend.PriceTrendId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(priceTrend);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PriceTrendExists(priceTrend.PriceTrendId))
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
            return View(priceTrend);
        }

        // GET: PriceTrends/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var priceTrend = await _context.PriceTrends
                .FirstOrDefaultAsync(m => m.PriceTrendId == id);
            if (priceTrend == null)
            {
                return NotFound();
            }

            return View(priceTrend);
        }

        // POST: PriceTrends/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var priceTrend = await _context.PriceTrends.FindAsync(id);
            _context.PriceTrends.Remove(priceTrend);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PriceTrendExists(int id)
        {
            return _context.PriceTrends.Any(e => e.PriceTrendId == id);
        }
    }
}
