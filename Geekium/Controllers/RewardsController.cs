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
    public class RewardsController : Controller
    {
        private readonly GeekiumContext _context;

        public RewardsController(GeekiumContext context)
        {
            _context = context;
        }

        // GET: Rewards
        public async Task<IActionResult> Index()
        {
            var geekiumContext = _context.Rewards.Include(r => r.Account);
            return View(await geekiumContext.ToListAsync());
        }

        // GET: Rewards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reward = await _context.Rewards
                .Include(r => r.Account)
                .FirstOrDefaultAsync(m => m.RewardId == id);
            if (reward == null)
            {
                return NotFound();
            }

            return View(reward);
        }

        // GET: Rewards/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "Email");
            return View();
        }

        // POST: Rewards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RewardId,AccountId,RewardType,RewardCode,PointCost,DateReceived")] Reward reward)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reward);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "Email", reward.AccountId);
            return View(reward);
        }

        // GET: Rewards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reward = await _context.Rewards.FindAsync(id);
            if (reward == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "Email", reward.AccountId);
            return View(reward);
        }

        // POST: Rewards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RewardId,AccountId,RewardType,RewardCode,PointCost,DateReceived")] Reward reward)
        {
            if (id != reward.RewardId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reward);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RewardExists(reward.RewardId))
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
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "Email", reward.AccountId);
            return View(reward);
        }

        // GET: Rewards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reward = await _context.Rewards
                .Include(r => r.Account)
                .FirstOrDefaultAsync(m => m.RewardId == id);
            if (reward == null)
            {
                return NotFound();
            }

            return View(reward);
        }

        // POST: Rewards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reward = await _context.Rewards.FindAsync(id);
            _context.Rewards.Remove(reward);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RewardExists(int id)
        {
            return _context.Rewards.Any(e => e.RewardId == id);
        }
    }
}
