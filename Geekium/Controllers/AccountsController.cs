using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Geekium.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace Geekium.Controllers
{
    public class AccountsController : Controller
    {
        private readonly GeekiumContext _context;

        public AccountsController(GeekiumContext context)
        {
            _context = context;
        }

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            return RedirectToAction("Index", "Home");
        }

        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        [HttpGet]
        public IActionResult Create(string returnUrl = "")
        {
            var model = new AccountViewModel { ReturnUrl = returnUrl };
            return View(model);
        }

        //Will retrieve the AccountViewModel data given by the user, create a new account object with that data, add that account
        //object to the Dbcontext, and perform a login with the supplied username and password
        [HttpPost]
        public async Task<IActionResult> Create(AccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                Account account = new Account();
                account.UserName = model.Username;
                account.UserPassword = model.Password;
                account.FirstName = model.FirstName;
                account.LastName = model.LastName;
                account.Email = model.Email;

                _context.Add(account);
                await _context.SaveChangesAsync();

                LoginViewModel loginModel = new LoginViewModel();
                loginModel.Username = model.Username;
                loginModel.Password = model.Password;

                await Login(loginModel);
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Invalid attempt");
            return View(model);
        }

        //Will retrieve the Login screen for the user and set the returnUrl accordingly so that the user is directed back to the
        //same page in the event that the login attempt has failed
        [HttpGet]
        public IActionResult Login(string returnUrl = "")
        {
            var model = new LoginViewModel { ReturnUrl = returnUrl };
            return View(model);
        }

        //Will retrieve the LoginViewModel data given by the user, check that the data corresponds with an existing account on the
        //database, create session objects for the account username and id, and direct the user to the Home page
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _context.Accounts.Any(m => m.UserName == model.Username && m.UserPassword == model.Password);

                if (result == true)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        foreach (var dbItem in _context.Accounts)
						{
                            if (dbItem.UserName == model.Username && dbItem.UserPassword == model.Password)
							{
                                HttpContext.Session.SetString("userId", dbItem.AccountId.ToString());
                                break;
                            }
						}

                        HttpContext.Session.SetString("username", model.Username);

                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            ModelState.AddModelError("", "Invalid attempt");
            return View(model);
        }

        //Will clear all session variables and return the user to the Home screen
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountId,UserName,UserPassword,FirstName,LastName,Email,PointBalance")] Account account)
        {
            if (id != account.AccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();

                    HttpContext.Session.SetString("username", account.UserName);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.AccountId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return View(account);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            await Logout();

            return RedirectToAction("Index", "Home");
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.AccountId == id);
        }
    }
}
