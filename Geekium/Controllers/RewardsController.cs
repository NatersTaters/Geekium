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
    public class RewardsController : Controller
    {
        private readonly GeekiumContext _context;

        private static Random random = new Random();

        public RewardsController(GeekiumContext context)
        {
            _context = context;
        }

        // GET: Rewards
        public async Task<IActionResult> Index()
        {
            string userId = HttpContext.Session.GetString("userId");
            string url = "/Accounts/Login";
            if(userId == null)
			{
                return LocalRedirect(url);
			}
            else
			{   
                var geekiumContext = _context.Rewards.Include(r => r.Account).Where(u => u.AccountId == int.Parse(userId));
                return View(await geekiumContext.ToListAsync());
            }
        }

        [HttpPost]
        public IActionResult SpendPoints()
        {
            return View();
        }

        public async Task<IActionResult> SpendPoints(int? id)
        {
            AccountsController accountsController = new AccountsController(_context);
            var account = await _context.Accounts.FindAsync(int.Parse(HttpContext.Session.GetString("userId")));

            Reward newReward = new Reward();
            newReward.AccountId = account.AccountId;
            newReward.DateReceived = DateTime.Now;

            int oldPointBalance = (int)account.PointBalance;
            int newPointBalance = 0;

            if (id == null)
			{
                return View();
			}
            else if(id == 1)
			{
                newReward.RewardType = "-25% Discount Code";
                newReward.RewardCode = RandomString();
                newReward.PointCost = 50;

                newPointBalance = oldPointBalance - 50;
            }
            else if(id == 2)
			{
                newReward.RewardType = "-50% Discount Code";
                newReward.RewardCode = RandomString();
                newReward.PointCost = 100;

                newPointBalance = oldPointBalance - 100;
            }
            else if(id == 3)
			{
                newReward.RewardType = "Free Website Merch";
                newReward.RewardCode = RandomString();
                newReward.PointCost = 150;

                newPointBalance = oldPointBalance - 150;
            }

            _context.Add(newReward);
            await _context.SaveChangesAsync();

            HttpContext.Session.SetInt32("pointsBalance", newPointBalance);

            await accountsController.EditPoints(account, newPointBalance);
            return RedirectToAction("Index");
        }

        public IActionResult UseReward()
        {
            return RedirectToAction("Index", "Carts", new { area = "" });
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

        //Will return a random string 32 characters in length to be used as a key for password encryption
        //and decryption
        public static string RandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 8)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
