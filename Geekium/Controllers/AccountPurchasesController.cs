using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Geekium.Models;
using Microsoft.AspNetCore.Http;

// SELLER NAME IS REDUNDANT IN THE MODEL
// DO WE CARE?

namespace Geekium.Controllers
{
    public class AccountPurchasesController : Controller
    {
        private readonly GeekiumContext _context;

        public AccountPurchasesController(GeekiumContext context)
        {
            _context = context;
        }

        // Return purchase history index
        public async Task<IActionResult> Index()
        {
            string userId = HttpContext.Session.GetString("userId");
            var geekiumContext = _context.AccountPurchases
                .Include(a => a.Account)
                .Include(a => a.Seller)
                .Where(a => a.AccountId.ToString() == userId);

            return View(await geekiumContext.ToListAsync());
        }

        // GET: AccountPurchases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountPurchase = await _context.AccountPurchases
                .Include(a => a.Account)
                .Include(a => a.Seller)
                .Include(a => a.Cart)
                .Include(a => a.Cart.ItemsForCart)
                .ThenInclude(a => a.SellListing)
                .FirstOrDefaultAsync(m => m.AccountPurchaseId == id);

            if (accountPurchase == null)
            {
                return NotFound();
            }

            return View(accountPurchase);
        }

        // GET: AccountPurchases/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "Email");
            ViewData["SellerId"] = new SelectList(_context.SellerAccounts, "SellerId", "SellerId");
            return View();
        }

        // POST: AccountPurchases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountPurchaseId,AccountId,SellerId,PurchaseDate,PurchasePrice,TrackingNumber,SellerName,PointsGained")] AccountPurchase accountPurchase)
        {
            if (ModelState.IsValid)
            {
                _context.Add(accountPurchase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "Email", accountPurchase.AccountId);
            ViewData["SellerId"] = new SelectList(_context.SellerAccounts, "SellerId", "SellerId", accountPurchase.SellerId);
            return View(accountPurchase);
        }


        private bool AccountPurchaseExists(int id)
        {
            return _context.AccountPurchases.Any(e => e.AccountPurchaseId == id);
        }
    }
}
