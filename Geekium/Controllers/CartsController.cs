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
                    .Include(c => c.Account)
                    .Where(s => s.TransactionComplete == false)
                    .FirstOrDefaultAsync(s => s.AccountId.ToString() == accountId);

                // If the account does not have a cart associated with them, create one
                if (cartContext == null)
				{
                    await CreateCart(accountId);
                    cartContext = await _context.Cart
                        .Where(s => s.TransactionComplete == false)
                        .FirstOrDefaultAsync(s => s.AccountId.ToString() == accountId);

                    // Calculate subtotal
                    double total = 0; // This is not updating properly
                    ViewBag.subTotal = total;
                    stripePay = 0;

                    // Calculate points
                    string points = PointsEarned(total).ToString();
                    ViewBag.points = PointsEarned(ViewBag.subTotal);

                    return View();
                }
                else
				{
                    // Find all the items associated with this cart
                    var cartItems = await _context.ItemsForCart
                        .Include(s => s.SellListing)
                        .Include(s => s.Cart)
                        .Where(s => s.SellListingId == s.SellListing.SellListingId)
                        .Where(s => s.CartId == cartContext.CartId)
                        .ToListAsync();

                    if (cartItems.Count != 0)
                    {
                        foreach (var item in cartItems)
                        {
                            if (item.Quantity > item.SellListing.SellQuantity)
                                item.Quantity = item.SellListing.SellQuantity;
                        }
                    }

                    // Calculate subtotal
                    double total = SubTotal(cartItems); // This is not updating properly
                    ViewBag.subTotal = total;
                    stripePay = (long)SubTotal(cartItems);

                    // Calculate points
                    string points = PointsEarned(total).ToString();
                    ViewBag.points = PointsEarned(ViewBag.subTotal);

                    return View(cartItems);
                }
            }
        }

        #region Cart Functions
        // Creates the cart
        public async Task<IActionResult> CreateCart(string id)
        {
            string userId = id;
            Cart cart = new Cart();
            cart.AccountId = int.Parse(userId);
            cart.TransactionComplete = false;
            cart.NumberOfProducts = 0;
            cart.TotalPrice = 0;
            cart.PointsGained = 0;

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
        public async Task<IActionResult> Add(int listingId, string userId)
        {
            string url = "/Accounts/Login";
            if (userId == null)
            {
                return LocalRedirect(url);
            }


            // Find the cart associated with this account
            var cartContext = await _context.Cart
                .Where(s => s.TransactionComplete == false)
                .FirstOrDefaultAsync(s => s.AccountId.ToString() == userId);

            // If the account does not have a cart associated with them, create one
            if (cartContext == null)
            {
                await CreateCart(userId);
                cartContext = await _context.Cart.
                    Where(s => s.TransactionComplete == false)
                    .FirstOrDefaultAsync(s => s.AccountId.ToString() == userId);
            }

            // Does this item already exist in this cart?
            var itemContext = await _context.ItemsForCart
                .Where(s => s.CartId == cartContext.CartId)
                .FirstOrDefaultAsync(s => s.SellListingId == listingId);

            if (itemContext == null)
            {
                ItemsForCart item = new ItemsForCart();
                item.CartId = cartContext.CartId;
                item.SellListingId = listingId;
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
                    if (!(quantity > itemContext.Quantity))
                    {
                        itemContext.Quantity = quantity;
                        _context.Update(itemContext);
                        await _context.SaveChangesAsync();
                    }
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

        //Update current cart item and set TransactionComplete to "true"
        public async Task ChangeCartTransactionStatus([Bind("CartId,AccountId,TransactionComplete,NumberOfProducts,TotalPrice,PointsGained")] Cart cart)
		{
            if (ModelState.IsValid)
            {
                cart.TransactionComplete = true;
                _context.Update(cart);
                await _context.SaveChangesAsync();
            }
        }

        //Delete any cart objects and items for cart objects from the database, to be used when the account is deleted
        public async Task<IActionResult> DeleteCart(int id)
        {
            var cart = await _context.Cart.FindAsync(id);

            var itemContext = await _context.ItemsForCart
                .Where(s => s.CartId == cart.CartId)
                .ToListAsync();

            foreach(var item in itemContext)
			{
                _context.ItemsForCart.Remove(item);
                await _context.SaveChangesAsync();
            }

            _context.Cart.Remove(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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

            // Calculate subtotal, tax, and total
            var model = await cartItems.ToListAsync();
            ViewBag.subTotal = SubTotal(model); // This is not updating properly
            ViewBag.tax = Tax(ViewBag.subTotal);
            ViewBag.total = TotalCost(ViewBag.subTotal, ViewBag.tax);
            ViewBag.totalStripe = TotalCost(ViewBag.subTotal, ViewBag.tax) * 100;

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

                // Find all the items associated with this cart
                var cartItems = _context.ItemsForCart
                    .Include(s => s.SellListing)
                    .Include(s => s.SellListing.Seller)
                    .Include(s => s.SellListing.Seller.Account)
                    .Include(s => s.Cart)
                    .Where(s => s.SellListingId == s.SellListing.SellListingId)
                    .Where(s => s.CartId == cartContext.CartId);

                var model = await cartItems.ToListAsync();
                long sAmount = (long)StripeSubTotal(model);
                long tax = (long)StripeTax(sAmount);
                long amount = (long)TotalCost(sAmount, tax);
            //Amount is being charged correctly with the decimal places, but doesn't want to display them 
            // (ex: 5082 should display $50.82 in email but only displays $50 instead)
            var charge = charges.Create(new ChargeCreateOptions
                {
                    Amount = amount,
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
                    HttpContext.Session.SetString("rewardType", "");
                    HttpContext.Session.SetString("rewardCode", "");
                }

                //Find account object with previous point balance
                var account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.AccountId == int.Parse(accountId));

                //Determing new points balance for the current account that is making the purchase
                double newPointBalance = (double)(account.PointBalance + amount);

                //Save the new point balance to the buyer account
                AccountsController accountsController = new AccountsController(_context, _hostEnvironment);
                await accountsController.EditPoints(account, (int)newPointBalance);

                //Save the new purchase item to the AccountPurchases Controller
                double purchasePrice = SubTotal(cartItems.ToList());
                int pointGain = (int)purchasePrice;

                AccountPurchasesController accountPurchases = new AccountPurchasesController(_context);
                await accountPurchases.AddPurchase(cartContext, purchasePrice, pointGain, int.Parse(accountId));

                //Update Cart Transaction Status
                await ChangeCartTransactionStatus(cartContext);

                return RedirectToAction("Index", "AccountPurchases", new { area = "" });
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
            foreach(var item in cart)
			{
                var amount = item.SellListing.SellPrice;
                string body = "Thank you for your recent purchase at Geekium: \n" + "Total: $" + amount + "\n  Product: " + item.SellListing.SellTitle + " - $" + item.SellListing.SellPrice;
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
                    Body = body
                })
                {
                    smtp.Send(message);
                }
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
             * Purchase by: BobRoss@gmail.com
             *
             */
        public void sellerNotifEmail(List<ItemsForCart> cart, Charge charge)
        {
            foreach(var item in cart)
			{
                string body = "One of your products have recently been sold: \n" + "Purchase by: " + HttpContext.Session.GetString("userEmail") + " \n  Product: " + item.SellListing.SellTitle + " - $" + item.SellListing.SellPrice;
                var email = item.SellListing.Seller.Account.Email;

                var sEmail = new MailAddress("geekium1234@gmail.com");
                var rEmail = new MailAddress(email.ToString()); //TODO: test
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
                    Body = body
                })
                {
                    smtp.Send(message);
                }
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
        //Stripe method of SubTotal
        public double StripeSubTotal(List<ItemsForCart> cart)
        {
            var totalPrice = cart.Sum(s => s.SellListing.SellPrice * s.Quantity) * 100;
            Math.Round(totalPrice, 2);
            try
            {
                if (HttpContext.Session.GetString("rewardType") != null && HttpContext.Session.GetString("rewardType") == "-25% Discount Code")
                {
                    var discount = (double)totalPrice * 0.25;
                    return (double)totalPrice - discount;
                }
                else if (HttpContext.Session.GetString("rewardType") != null && HttpContext.Session.GetString("rewardType") == "-50% Discount Code")
                {
                    var discount = (double)totalPrice * 0.50;
                    return (double)totalPrice - discount;
                }
                else if (HttpContext.Session.GetString("rewardType") != null && HttpContext.Session.GetString("rewardType") == "-75% Discount Code")
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
        public double Tax(double subTotal)
        {
            double tax = 0.13;
            //double totalPrice = cart.Sum(s => s.SellListing.SellPrice * s.Quantity);
            return Math.Round(subTotal * tax, 2);
        }
        //Stripe method of tax
        public double StripeTax(double subTotal)
        {
            double tax = 0.13;
            double taxAmount = Math.Round(subTotal * tax, 0, MidpointRounding.AwayFromZero);
            double totalTax = taxAmount;
            //double totalPrice = cart.Sum(s => s.SellListing.SellPrice * s.Quantity);
            //return Math.Round(subTotal * tax, 2);
            return totalTax;
        }

        public double TotalCost(double subTotal, double tax)
        {
            double total = subTotal + tax;
            return total;
        }

        public double StripeAmount(double total)
        {
            double stripeRate = 100;
            return total * stripeRate;
        }

        //Points are calculated by totalPoints(total cost of items) times the point rate of 10 and is rounded to the nearest whole number away from zero ($21.47 = 215 points)
        public double PointsEarned(double totalPoints)
        {
            double pRate = 10;
            return Math.Round(totalPoints * pRate, 0, MidpointRounding.AwayFromZero);
        }
        #endregion
    }
}
