using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Geekium.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Net.Mail;
using System.Net;

namespace Geekium.Controllers
{
    public class AccountsController : Controller
    {
        private readonly GeekiumContext _context;

        private static Random random = new Random();

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
                foreach (var dbItem in _context.Accounts)
                {
                    if (dbItem.UserName == model.Username)
                    {
                        ModelState.AddModelError("", "Username already taken");
                        return View(model);
                    }
                    else if (dbItem.Email == model.Email)
					{
                        ModelState.AddModelError("", "Email already taken");
                        return View(model);
                    }
                }

                var originalPassword = model.Password;
                string key = RandomString();

                var encryptedPassword = EncryptString(originalPassword, key);

                Account account = new Account();
                account.UserName = model.Username;
                account.UserPassword = encryptedPassword.ToString();
                account.PaswordHash = key.ToString();
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
                var account = await _context.Accounts.FirstOrDefaultAsync(m => m.UserName == model.Username);
                if (account != null)
				{
					var decryptedPassword = DecryptString(account.UserPassword, account.PaswordHash);

					if (decryptedPassword == model.Password)
					{
						HttpContext.Session.SetString("username", account.UserName);
						HttpContext.Session.SetString("userId", account.AccountId.ToString());
                        HttpContext.Session.SetString("userEmail", account.Email);

						var sellerAccount = await _context.SellerAccounts.FirstOrDefaultAsync(m => m.AccountId == account.AccountId);
						if (sellerAccount != null)
						{
                            HttpContext.Session.SetInt32("sellerId", sellerAccount.SellerId);
                        }

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

        //Will direct user to the Upgrade page and set the returnUrl variable if an error has occured
        [HttpGet]
        public IActionResult Upgrade(string returnUrl = "")
        {
            SendEmail();

			var model = new UpgradeViewModel { ReturnUrl = returnUrl };
            return View(model);
        }

        //Will check if the code sent by email matches the code generated in the application, and will create a new SellerAccount
        //object using the account credidentials
        [HttpPost]
        public async Task<IActionResult> Upgrade(UpgradeViewModel model)
        {
            if (model.Code == HttpContext.Session.GetString("emailCode"))
			{
                SellerAccount sellerAccount = new SellerAccount();
                sellerAccount.AccountId = Int32.Parse(HttpContext.Session.GetString("userId"));
                HttpContext.Session.SetInt32("sellerId", sellerAccount.SellerId);

                SellerAccountsController sellerAccountsController = new SellerAccountsController(_context);
                await sellerAccountsController.Create(sellerAccount);

                return View("UpgradeSuccess");
            }
            ModelState.AddModelError("", "Code is invalid");
            return View(model);
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
        public async Task<IActionResult> Edit(int id, [Bind("AccountId,UserName,UserPassword,PaswordHash,FirstName,LastName,Email,PointBalance")] Account account)
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

        //Will return a random string 32 characters in length to be used as a key for password encryption
        //and decryption
        public static string RandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 32)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        //Will encrypt a provided string using a provided key
        public static string EncryptString(string text, string keyString)
        {
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }

                        var iv = aesAlg.IV;

                        var decryptedContent = msEncrypt.ToArray();

                        var result = new byte[iv.Length + decryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }

        //Will decrypt a string using a provided key
        public static string DecryptString(string cipherText, string keyString)
        {
            var fullCipher = Convert.FromBase64String(cipherText);

            var iv = new byte[16];
            var cipher = new byte[16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }

        //Will generate a random code and connect to the SMTP server provided by Gmail, and will use a email account to
        //send the random code to the user with email
        public void SendEmail()
		{
            Random r = new Random();
            int randNum = r.Next(1000000);
            string emailCode = randNum.ToString("D6");
            HttpContext.Session.SetString("emailCode", emailCode);

            var senderEmail = new MailAddress("geekium1234@gmail.com");
            var receiverEmail = new MailAddress(HttpContext.Session.GetString("userEmail"));
            var password = "geekiumaccount1234";
            var sub = "Account Verification";
            var body = "Your one time is code is: " + emailCode + ", please enter it where prompted on the website";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, password)
            };
            using (var mess = new MailMessage(senderEmail, receiverEmail)
            {
                Subject = sub,
                Body = body
            })
            {
                smtp.Send(mess);
            }
        }
    }
}
