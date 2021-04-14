using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Geekium.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace Geekium.Controllers
{
    public class SellerAccountsController : Controller
    {
        private readonly GeekiumContext _context;
        private IWebHostEnvironment _hostEnvironment;

        public SellerAccountsController(GeekiumContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

		// GET: SellerAccounts
		public async Task<IActionResult> Index()
        {
            var geekiumContext = _context.SellerAccounts.Include(s => s.Account);
            return View(await geekiumContext.ToListAsync());
        }

        // GET: SellerAccounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sellerAccount = await _context.SellerAccounts
                .Include(s => s.Account)
                .FirstOrDefaultAsync(m => m.SellerId == id);
            if (sellerAccount == null)
            {
                return NotFound();
            }

            return View(sellerAccount);
        }

        // GET: SellerAccounts/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "Email");
            return View();
        }

        // POST: SellerAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SellerId,AccountId,AverageRating")] SellerAccount sellerAccount)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sellerAccount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "Email", sellerAccount.AccountId);
            return View(sellerAccount);
        }

        // GET: SellerAccounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sellerAccount = await _context.SellerAccounts.FindAsync(id);
            if (sellerAccount == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "Email", sellerAccount.AccountId);
            return View(sellerAccount);
        }

        // POST: SellerAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SellerId,AccountId,AverageRating")] SellerAccount sellerAccount)
        {
            if (id != sellerAccount.SellerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sellerAccount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SellerAccountExists(sellerAccount.SellerId))
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
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "Email", sellerAccount.AccountId);
            return View(sellerAccount);
        }

        // GET: SellerAccounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sellerAccount = await _context.SellerAccounts
                .Include(s => s.Account)
                .FirstOrDefaultAsync(m => m.SellerId == id);
            if (sellerAccount == null)
            {
                return NotFound();
            }

            return View(sellerAccount);
        }

        // POST: SellerAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sellerAccount = await _context.SellerAccounts.FindAsync(id);

            var sellListing = await _context.SellListings
                .Include(s => s.PriceTrend)
                .Include(s => s.Seller)
                .Include(s => s.Seller.Account)
                .FirstOrDefaultAsync(s => s.Seller.SellerId == id);

            var tradeListing = await _context.TradeListings
                .Include(t => t.Seller)
                .Include(t => t.Seller.Account)
                .FirstOrDefaultAsync(t => t.Seller.SellerId == id);

            var sellerReview = await _context.SellerReviews
                .Include(s => s.Seller)
                .Include(s => s.Seller.Account)
                .FirstOrDefaultAsync(s => s.Seller.SellerId == id);

            if (sellListing != null)
            {
                MyListingsController myListings = new MyListingsController(_context, _hostEnvironment);
                await myListings.DeleteConfirmedSelling(sellListing.SellListingId);
            }

            if (tradeListing != null)
            {
                MyListingsController myListings = new MyListingsController(_context, _hostEnvironment);
                await myListings.DeleteConfirmedTrade(tradeListing.TradeListingId);
            }

            if (sellerReview != null)
			{
                SellerReviewsController sellerReviews = new SellerReviewsController(_context);
                await sellerReviews.DeleteConfirmed(sellerReview.SellerReviewId);
			}

            _context.SellerAccounts.Remove(sellerAccount);
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SellerAccountExists(int id)
        {
            return _context.SellerAccounts.Any(e => e.SellerId == id);
        }
    }
}
