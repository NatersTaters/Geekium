﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Geekium.Models;

namespace Geekium.Controllers
{
    public class AccountPurchasesController : Controller
    {
        private readonly GeekiumContext _context;

        public AccountPurchasesController(GeekiumContext context)
        {
            _context = context;
        }

        // GET: AccountPurchases
        public async Task<IActionResult> Index()
        {
            var geekiumContext = _context.AccountPurchases.Include(a => a.Account).Include(a => a.Seller);
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

        // GET: AccountPurchases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountPurchase = await _context.AccountPurchases.FindAsync(id);
            if (accountPurchase == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "Email", accountPurchase.AccountId);
            ViewData["SellerId"] = new SelectList(_context.SellerAccounts, "SellerId", "SellerId", accountPurchase.SellerId);
            return View(accountPurchase);
        }

        // POST: AccountPurchases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountPurchaseId,AccountId,SellerId,PurchaseDate,PurchasePrice,TrackingNumber,SellerName,PointsGained")] AccountPurchase accountPurchase)
        {
            if (id != accountPurchase.AccountPurchaseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(accountPurchase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountPurchaseExists(accountPurchase.AccountPurchaseId))
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
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "Email", accountPurchase.AccountId);
            ViewData["SellerId"] = new SelectList(_context.SellerAccounts, "SellerId", "SellerId", accountPurchase.SellerId);
            return View(accountPurchase);
        }

        // GET: AccountPurchases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountPurchase = await _context.AccountPurchases
                .Include(a => a.Account)
                .Include(a => a.Seller)
                .FirstOrDefaultAsync(m => m.AccountPurchaseId == id);
            if (accountPurchase == null)
            {
                return NotFound();
            }

            return View(accountPurchase);
        }

        // POST: AccountPurchases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var accountPurchase = await _context.AccountPurchases.FindAsync(id);
            _context.AccountPurchases.Remove(accountPurchase);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountPurchaseExists(int id)
        {
            return _context.AccountPurchases.Any(e => e.AccountPurchaseId == id);
        }
    }
}
