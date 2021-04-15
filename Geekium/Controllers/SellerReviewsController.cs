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
            if (TempData.ContainsKey("Message"))
            {
                ViewBag.Error = TempData["Message"].ToString();
                string redirectedId = HttpContext.Session.GetString("sellerid");
                id = Convert.ToInt32(redirectedId);
            }

            // All the reviews
            var reviewContext = _context.SellerReviews
                .Include(s => s.Seller)
                .Include(s => s.Seller.Account)
                .Include(s => s.Account)
                .Where(s => s.Seller.SellerId == id);

            // The seller account associated with these reviews
            var accountContext = await _context.SellerAccounts
                .Include(s => s.Account)
                .FirstOrDefaultAsync(s => s.SellerId == id);
           
            if (accountContext == null)
            {
                string redirectedId = HttpContext.Session.GetString("sellerid");
                id = Convert.ToInt32(redirectedId);
                accountContext = await _context.SellerAccounts
                    .Include(s => s.Account)
                    .FirstOrDefaultAsync(s => s.SellerId == id);
            }

            if (reviewContext.ToList().Count > 0)
                ViewBag.Average = Math.Round((double)reviewContext.Average(s => s.BuyerRating), 2);
            else
                ViewBag.Average = "N/A";

            HttpContext.Session.SetString("sellerid", id.ToString());
            ViewBag.Seller = accountContext.Account.UserName;

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

        // Only accounts that have bought from this seller can make a review
        // Only one review per account per seller
        public async Task<IActionResult> Create()
        {
            string userId = HttpContext.Session.GetString("userId");
            string url = "/Accounts/Login";
            if (userId == null)
                return LocalRedirect(url);

            string id = HttpContext.Session.GetString("sellerid");

            // Figure out if they have a review on this person already
            var allowReview = await _context.SellerReviews
                .Include(s => s.Seller)
                .Include(s => s.Seller.Account)
                .Include(s => s.Account)
                .Where(s => s.Seller.SellerId.ToString() == id)
                .FirstOrDefaultAsync(s => s.AccountId.ToString() == userId);

            var sellerName = await _context.SellerReviews
                .Include(s => s.Seller)
                .Include(s => s.Seller.Account)
                .FirstOrDefaultAsync(s => s.Seller.SellerId.ToString() == id);

            // Optional: figure out if the reviewer actually bought something from seller

            if (allowReview != null)
            {
                TempData["Message"] = "You already left a review for this person!";
                return RedirectToAction(nameof(Index));
            }

            ViewData["SellerId"] = new SelectList(_context.SellerAccounts, "SellerId", "SellerId");
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId");
            ViewData["SellerName"] = sellerName.Seller.Account.UserName;
            return View();
        }

        // POST: SellerReviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SellerReviewId,AccountId,SellerId,BuyerRating,ReviewDescription")] SellerReview sellerReview)
        {
            string userId = HttpContext.Session.GetString("userId");
            var account = await _context.Accounts
                .FirstOrDefaultAsync(s => s.AccountId.ToString() == userId);

            if (ModelState.IsValid)
            {
                sellerReview.Account = account;
                sellerReview.AccountId = account.AccountId;
                _context.Add(sellerReview);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Successfully added review!";

                var sellerId = sellerReview.SellerId;

                // All the reviews
                var reviewContext = _context.SellerReviews
                    .Include(s => s.Seller)
                    .Include(s => s.Seller.Account)
                    .Include(s => s.Account)
                    .Where(s => s.Seller.SellerId == sellerId);

                var sellerAccount = await _context.SellerAccounts
                    .Include(s => s.Account)
                    .FirstOrDefaultAsync(s => s.SellerId == sellerId);

                sellerAccount.AverageRating = Math.Round((double)reviewContext.Average(s => s.BuyerRating), 2);
                _context.Update(sellerAccount);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }


            ViewData["SellerId"] = new SelectList(_context.SellerAccounts, "SellerId", "SellerId", sellerReview.SellerId);
            return View(sellerReview);
        }

        // The user will be able to change any personalized reviews
        public async Task<IActionResult> Edit()
        {
            string userId = HttpContext.Session.GetString("userId");
            string url = "/Accounts/Login";
            if (userId == null)
                return LocalRedirect(url);

            string id = HttpContext.Session.GetString("sellerid");

            var sellerReview = await _context.SellerReviews
                .Include(s => s.Seller.Account)
                .FirstOrDefaultAsync(s => s.AccountId.ToString() == userId);

            if (sellerReview == null)
            {
                TempData["Message"] = "Review not found. Please create one!";
                return RedirectToAction(nameof(Index));
            }

            ViewData["SellerId"] = new SelectList(_context.SellerAccounts, "SellerId", "SellerId", sellerReview.SellerId);
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId");
            return View(sellerReview);
        }

        // POST: SellerReviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SellerReviewId,SellerId,BuyerRating,ReviewDescription")] SellerReview sellerReview)
        {
            string userId = HttpContext.Session.GetString("userId");
            var account = await _context.Accounts
                .FirstOrDefaultAsync(s => s.AccountId.ToString() == userId);

            if (ModelState.IsValid)
            {
                try
                {
                    sellerReview.Account = account;
                    sellerReview.AccountId = account.AccountId;
                    _context.Update(sellerReview);
                    await _context.SaveChangesAsync();

                    var sellerId = sellerReview.SellerId;

                    // All the reviews
                    var reviewContext = _context.SellerReviews
                        .Include(s => s.Seller)
                        .Include(s => s.Seller.Account)
                        .Include(s => s.Account)
                        .Where(s => s.Seller.SellerId == sellerId);

                    var sellerAccount = await _context.SellerAccounts
                        .Include(s => s.Account)
                        .FirstOrDefaultAsync(s => s.SellerId == sellerId);

                    sellerAccount.AverageRating = Math.Round((double)reviewContext.Average(s => s.BuyerRating), 2);
                    _context.Update(sellerAccount);
                    await _context.SaveChangesAsync();

                    TempData["Message"] = "Successfully edited review!";
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
        public async Task<IActionResult> Delete()
        {
            string userId = HttpContext.Session.GetString("userId");
            string url = "/Accounts/Login";
            if (userId == null)
                return LocalRedirect(url);

            string id = HttpContext.Session.GetString("sellerid");

            var sellerReview = await _context.SellerReviews
                .Include(s => s.Seller)
                .Include(s => s.Seller.Account)
                .FirstOrDefaultAsync(m => m.AccountId.ToString() == userId);

            if (sellerReview == null)
            {
                TempData["Message"] = "Review not found. Please create one!";
                return RedirectToAction(nameof(Index));
            }

            return View(sellerReview);
        }

        // POST: SellerReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string userId = HttpContext.Session.GetString("userId");

            var sellerReview = await _context.SellerReviews
                .FirstOrDefaultAsync(s => s.AccountId.ToString() == userId);

            _context.SellerReviews.Remove(sellerReview);
            await _context.SaveChangesAsync();
            TempData["Message"] = "Review successfully deleted!";

            var sellerId = sellerReview.SellerId;

            // All the reviews
            var reviewContext = _context.SellerReviews
                .Include(s => s.Seller)
                .Include(s => s.Seller.Account)
                .Include(s => s.Account)
                .Where(s => s.Seller.SellerId == sellerId);

            var sellerAccount = await _context.SellerAccounts
                .Include(s => s.Account)
                .FirstOrDefaultAsync(s => s.SellerId == sellerId);

            sellerAccount.AverageRating = Math.Round((double)reviewContext.Average(s => s.BuyerRating), 2);
            _context.Update(sellerAccount);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool SellerReviewExists(int id)
        {
            return _context.SellerReviews.Any(e => e.SellerReviewId == id);
        }
    }
}
