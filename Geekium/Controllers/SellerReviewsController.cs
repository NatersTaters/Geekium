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
    public class SellerReviewsController : Controller
    {
        private readonly GeekiumContext _context;

        public SellerReviewsController(GeekiumContext context)
        {
            _context = context;
        }

        // First we will open all the reviews related to this account
        public async Task<IActionResult> Index(int id)
        {
            // All the reviews
            var reviewContext = _context.SellerReviews
                .Include(s => s.Seller)
                .Include(s => s.Seller.Account)
                .Where(s => s.Seller.SellerId == id);

            // The seller account associated with these reviews
            var accountContext = await _context.SellerReviews
                .Include(s => s.Seller)
                .Include(s => s.Seller.Account)
                .FirstOrDefaultAsync(s => s.SellerId == id);


            // We currently do not keep track of who made this review
            try
            {
                ViewBag.Average = Math.Round((decimal)reviewContext.Average(s => s.BuyerRating), 2);
            }
            catch (Exception)
            {
                ViewBag.Average = "N/A";
            }
            ViewBag.Average = Math.Round((decimal)reviewContext.Average(s => s.BuyerRating), 2);
            ViewBag.Seller = accountContext.Seller.Account.UserName;

            return View(await reviewContext.ToListAsync());
        }

        // GET: SellerReviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sellerReview = await _context.SellerReviews
                .Include(s => s.Seller)
                .FirstOrDefaultAsync(m => m.SellerReviewId == id);
            if (sellerReview == null)
            {
                return NotFound();
            }

            return View(sellerReview);
        }

        // The buyer can only create a review through their purchase history
        // Even after refunding, the review will stay
        // Only one review per account
        public IActionResult Create()
        {
            ViewData["SellerId"] = new SelectList(_context.SellerAccounts, "SellerId", "SellerId");
            return View();
        }

        // POST: SellerReviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SellerReviewId,SellerId,BuyerRating,ReviewDescription")] SellerReview sellerReview)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sellerReview);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SellerId"] = new SelectList(_context.SellerAccounts, "SellerId", "SellerId", sellerReview.SellerId);
            return View(sellerReview);
        }

        // The user will be able to change any personalized reviews
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sellerReview = await _context.SellerReviews.FindAsync(id);
            if (sellerReview == null)
            {
                return NotFound();
            }
            ViewData["SellerId"] = new SelectList(_context.SellerAccounts, "SellerId", "SellerId", sellerReview.SellerId);
            return View(sellerReview);
        }

        // POST: SellerReviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SellerReviewId,SellerId,BuyerRating,ReviewDescription")] SellerReview sellerReview)
        {
            if (id != sellerReview.SellerReviewId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sellerReview);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SellerReviewExists(sellerReview.SellerReviewId))
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
            ViewData["SellerId"] = new SelectList(_context.SellerAccounts, "SellerId", "SellerId", sellerReview.SellerId);
            return View(sellerReview);
        }

        // User should be able to delete their reviews
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sellerReview = await _context.SellerReviews
                .Include(s => s.Seller)
                .FirstOrDefaultAsync(m => m.SellerReviewId == id);
            if (sellerReview == null)
            {
                return NotFound();
            }

            return View(sellerReview);
        }

        // POST: SellerReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sellerReview = await _context.SellerReviews.FindAsync(id);
            _context.SellerReviews.Remove(sellerReview);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SellerReviewExists(int id)
        {
            return _context.SellerReviews.Any(e => e.SellerReviewId == id);
        }
    }
}
