using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Geekium.Models;
using Microsoft.AspNetCore.Http;
using Geekium.Helpers;
using Stripe;
using System.Net.Mail;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Hosting;

namespace Geekium.Controllers
{
    public class CartsController : Controller
    {
        private readonly GeekiumContext _context;
        private long stripePay;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CartsController(GeekiumContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // Get contents of cart and return index page
        public async Task<IActionResult> Index()
        {
            string accountId = HttpContext.Session.GetString("userId"); 
            string url = "/Accounts/Login";
            if(accountId == null)
            {
                return LocalRedirect(url);
            }
            else
            {
                // Find the cart associated with this account
                var cartContext = await _context.Cart
                    .Where(s => s.TransactionComplete == false)
                    .FirstOrDefaultAsync(s => s.AccountId.ToString() == accountId);

                // If the account does not have a cart associated with them, create one
                if (cartContext == null)
                    await CreateCart();

                // Find all the items associated with this cart
                var cartItems = _context.ItemsForCart
                    .Include(s => s.SellListing)
                    .Include(s => s.Cart)
                    .Where(s => s.SellListingId == s.SellListing.SellListingId)
                    .Where(s => s.CartId == cartContext.CartId);

                if (cartItems != null)
				{
                    foreach (var item in cartItems)
                    {
                        if (item.Quantity > item.SellListing.SellQuantity)
                            item.Quantity = item.SellListing.SellQuantity;
                    }
                }
                
                // Calculate subtotal
                var model = await cartItems.ToListAsync();
                double total = SubTotal(model); // This is not updating properly
                ViewBag.subTotal = total;
                stripePay = (long)SubTotal(model);

                // Calculate points
                //ViewBag.points = PointsEarned(ViewBag.subTotal);
                string points = PointsEarned(total).ToString();
                HttpContext.Session.SetString("points", points);

                return View(model);
            }
        }

        #region Cart Functions
        // Creates the cart
        public async Task<IActionResult> CreateCart()
        {
            string userId = HttpContext.Session.GetString("userId");
            Cart cart = new Cart();
            cart.AccountId = int.Parse(userId);
            cart.TransactionComplete = false;
            cart.NumberOfProducts = 0;
            cart.TotalPrice = 0;
            cart.PointsGained = 0;

            // We need the seller ID associated with this account ID

            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return RedirectToAction("Index");
        }

        // Check if cart exists
        private bool CartExists(int id)
        {
            return _context.Cart.Any(e => e.CartId == id);
        }

        // Removing item from cart
        public async Task<IActionResult> Remove(int id)
        {
            // Find the corresponding item to remove from cart
            var itemToRemove = await _context.ItemsForCart.FindAsync(id);
            _context.ItemsForCart.Remove(itemToRemove);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // Adding the item to cart
        public async Task<IActionResult> Add(int id)
        {
            string accountId = HttpContext.Session.GetString("userId");
            string url = "/Accounts/Login";
            if (accountId == null)
            {
                return LocalRedirect(url);
            }

            // Find the cart associated with this account
            var cartContext = await _context.Cart
                .Where(s => s.TransactionComplete == false)
                .FirstOrDefaultAsync(s => s.AccountId.ToString() == accountId);

            // Does this item already exist in this cart?
            var itemContext = await _context.ItemsForCart
                .Where(s => s.CartId == cartContext.CartId)
                .FirstOrDefaultAsync(s => s.SellListingId == id);

            if (itemContext == null)
            {
                ItemsForCart item = new ItemsForCart();
                item.CartId = cartContext.CartId;
                item.SellListingId = id;
                item.Quantity = 1;

                if (ModelState.IsValid)
                {
                    _context.Add(item);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                // Find the associated cart item
                itemContext.Quantity++;
                _context.Update(itemContext);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        // Updating quantity of item from cart page
        public async Task<IActionResult> UpdateQuantity(int id, int quantity)
        {
            string accountId = HttpContext.Session.GetString("userId");
            string url = "/Accounts/Login";
            if (accountId == null)
            {
                return LocalRedirect(url);
            }

            // Find the cart associated with this account
            var cartContext = await _context.Cart
                .Where(s => s.TransactionComplete == false)
                .FirstOrDefaultAsync(s => s.AccountId.ToString() == accountId);

            // Find the associated cart item
            var itemContext = await _context.ItemsForCart
                .Where(s => s.CartId == cartContext.CartId)
                .FirstOrDefaultAsync(s => s.ItemsForCartId == id);

            // Find all the items associated with this cart
            var cartItems = _context.ItemsForCart
                .Include(s => s.SellListing)
                .Include(s => s.Cart)
                .Where(s => s.SellListingId == s.SellListing.SellListingId)
                .Where(s => s.CartId == cartContext.CartId);

            if (ModelState.IsValid)
            {
                try
                {
                    itemContext.Quantity = quantity;
                    _context.Update(itemContext);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // I probably should do this for itemforcart instead of cart itself
                    if (!CartExists(cartContext.CartId))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Checkout Functions
        //Payment page
        public async Task<IActionResult> CartPayment()
        {
            // Find the cart associated with this account
            var cartContext = await _context.Cart
                .Where(s => s.TransactionComplete == false)
                .FirstOrDefaultAsync(s => s.AccountId.ToString() == HttpContext.Session.GetString("userId"));

            // Find all the items associated with this cart
            var cartItems = _context.ItemsForCart
                .Include(s => s.SellListing)
                .Include(s => s.Cart)
                .Where(s => s.SellListingId == s.SellListing.SellListingId)
                .Where(s => s.CartId == cartContext.CartId);

            // Calculate subtotal
            var model = await cartItems.ToListAsync();
            ViewBag.subTotal = SubTotal(model); // This is not updating properly
            ViewBag.subTotalStripe = SubTotal(model) * 100;

            return View();
        }

        //Checkout - THIS IS WHERE PAYMENT PORTAL
        //public async Task<IActionResult> CheckOut()
            public async Task<IActionResult> CheckOut(string stripeEmail, string stripeToken, bool charged)
            {
                var customers = new CustomerService();
                var charges = new ChargeService();

                var customer = customers.Create(new CustomerCreateOptions{ 
                    Email=stripeEmail,
                    Source=stripeToken
                });

                // Find the cart associated with this account
                var cartContext = await _context.Cart
                .Where(s => s.TransactionComplete == false)
                .FirstOrDefaultAsync(s => s.AccountId.ToString() == HttpContext.Session.GetString("userId"));

                // If the account does not have a cart associated with them, create one
                if (cartContext == null)
                await CreateCart();

                // Find all the items associated with this cart
                var cartItems = _context.ItemsForCart
                .Include(s => s.SellListing)
                .Include(s => s.Cart)
                .Where(s => s.SellListingId == s.SellListing.SellListingId)
                .Where(s => s.CartId == cartContext.CartId);

                var model = await cartItems.ToListAsync();
                long sAmount = (long)SubTotal(model) * 100;

                var charge = charges.Create(new ChargeCreateOptions
                {
                    Amount = sAmount,
                    Description = "Purchase off of Geekium",
                    Currency = "cad",
                    Customer = customer.Id,
                    ReceiptEmail = stripeEmail,
                    Metadata=new Dictionary<string, string>()
                    {
                        {"OrderId","111" },
                        {"OrderFrom", "Geekium" }
                    }
                });

                string accountId = HttpContext.Session.GetString("userId");

            if (charge.Status == "succeeded")
            {
                string BalanceTransactionId = charge.BalanceTransactionId;
                customerNotifEmail(model, charge);
                sellerNotifEmail(model, charge);

                //Set session objects for reward type and reward code to null to avoid redundency when
                //a new cart object is created
                if ((HttpContext.Session.GetString("rewardType")) != null)
				{
                    HttpContext.Session.SetString("rewardType", null);
                    HttpContext.Session.SetString("rewardCode", null);
                }

                //Find account object with previous point balance and add points earned to the account point balance
                var account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.AccountId == int.Parse(accountId));

                double newPointBalance = (double)(account.PointBalance + PointsEarned(charge.Amount));

                AccountsController accountsController = new AccountsController(_context, _hostEnvironment);
                await accountsController.EditPoints(account, (int)newPointBalance);

                return RedirectToAction("Create", "AccountPurchases", new { area = "" });

            }
                else
                {
                    return View("Unsuccessful");
                }
            }
            /* Customer email, layout as:
             *-----------------------------------
             *
             *Subject: Purchase Confirmation
             *Sender: geekium1234@gmail.com
             *
             *Body:  
             *Thank you for your recent purchase at Geekium: 
             * Purchase of: Geekium T-shirt - $19
             * Purchase of: Item 2 - $20
             * Purchase of: Item 3 - $35 
             *Total: $74 
             *
             *
             */

        public void customerNotifEmail(List<ItemsForCart> cart, Charge charge)
        {
            //Initialize new instance of stringbuilder
            StringBuilder sb = new StringBuilder();

            //Since Stripes pricing is 100 = $1, 1000 = $10, 10000 = $100, the charge.Amount is divided by 100 to get proper pricing
            //So instead of the output being:
            //Total: $500 
            //It will display as 
            //Total: $5
            var amount = charge.Amount / 100;


            var body = "Thank you for your recent purchase at Geekium: 0 \n" + "Total: $" + amount;
            sb.Append(body);
            for (int i = 0; i < cart.Count; i++)
            {
                sb.Insert(sb.ToString().IndexOf("0 "), " \n  Purchase of: " + cart[i].SellListing.SellTitle + " - $" + cart[i].SellListing.SellPrice);
            }
            var sEmail = new MailAddress("geekium1234@gmail.com");
            var rEmail = new MailAddress(HttpContext.Session.GetString("userEmail"));
            var password = "geekiumaccount1234";
            var sub = "Purchase Confirmation";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(sEmail.Address, password)
            };
            using (var message = new MailMessage(sEmail, rEmail)
            {
                Subject = sub,
                Body = sb.ToString()
            })
            {
                smtp.Send(message);
            }
        }
             /* Seller email, layout as:
             *-----------------------------------
             *
             *Subject: Product Sold
             *Sender: geekium1234@gmail.com
             *
             *Body:  
             *One of your products have recently been sold: 
             * Product: Geekium T-shirt - 19
             * Product: Item 2 - 20
             * Product: Item 3 - 35 
             *
             *
             */
        public void sellerNotifEmail(List<ItemsForCart> cart, Charge charge)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbEmail = new StringBuilder();

            var body = "One of your products have recently been sold: 0 \n";
            var email = "Placeholder";

            sb.Append(body);
            sbEmail.Append(email);
            for (int i = 0; i < cart.Count; i++)
            {
                sb.Insert(sb.ToString().IndexOf("0 "), " \n  Product: " + cart[i].SellListing.SellTitle + " - $" + cart[i].SellListing.SellPrice);
                sbEmail.Insert(sb.ToString().IndexOf("Placeholder "), cart[i].SellListing.Seller.Account.Email);
            }
            var sEmail = new MailAddress("geekium1234@gmail.com");
            var rEmail = new MailAddress(email); //TODO: Find out why it's returning null
            var password = "geekiumaccount1234";
            var sub = "Purchase Confirmation";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(sEmail.Address, password)
            };
            using (var message = new MailMessage(sEmail, rEmail)
            {
                Subject = sub,
                Body = sb.ToString()
            })
            {
                smtp.Send(message);
            }
        }
        #endregion

        #region Helper Functions
        /*Cost Calculations for: Price, Tax, and total*/
        public double SubTotal(List<ItemsForCart> cart)
        {
            var totalPrice = cart.Sum(s => s.SellListing.SellPrice * s.Quantity);
            Math.Round(totalPrice, 2);
            try
            {
                if(HttpContext.Session.GetString("rewardType") != null && HttpContext.Session.GetString("rewardType") == "-25% Discount Code")
				{
                    var discount = (double)totalPrice * 0.25;
                    return (double)totalPrice - discount;
                }
                else if(HttpContext.Session.GetString("rewardType") != null && HttpContext.Session.GetString("rewardType") == "-50% Discount Code")
				{
                    var discount = (double)totalPrice * 0.50;
                    return (double)totalPrice - discount;
                }
                else if(HttpContext.Session.GetString("rewardType") != null && HttpContext.Session.GetString("rewardType") == "-75% Discount Code")
				{
                    var discount = (double)totalPrice * 0.75;
                    return (double)totalPrice - discount;
                }
                else
				{
                    return (double)totalPrice;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception caught {0}", e);
                throw e;
            }
        }

        public double Tax(double firstTotal)
        {
            double tax = 0.13;
            return Math.Round(firstTotal * tax, 2);
        }

        public double TotalCost(double totalWithoutTax, double tax)
        {
            return totalWithoutTax + tax;
        }

        public double StripeAmount(double total)
        {
            double stripeRate = 100;
            return total * stripeRate;
        }

        //Points are calculated by totalPoints(total cost of items) times the point rate of 10 and is rounded to the nearest whole number away from zero 
        public double PointsEarned(double totalPoints)
        {
            double pRate = 10;
            return Math.Round(totalPoints * pRate, 0, MidpointRounding.AwayFromZero);
        }
        #endregion
    }
}
