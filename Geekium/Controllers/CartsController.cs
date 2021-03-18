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

namespace Geekium.Controllers
{
    public class CartsController : Controller
    {
        private readonly GeekiumContext _context;

        public CartsController(GeekiumContext context)
        {
            _context = context;
        }

        // GET: Carts
        public IActionResult Index()
        {
            string accountId = HttpContext.Session.GetString("AccountId"); //TODO: AccountId is returning always null
            string url = "/Accounts/Login";
            if(accountId == null)
            {
                return LocalRedirect(url);
            }
            else
            {
                var cart = SessionHelper.GetObjectFromJson<List<ItemsForCart>>(HttpContext.Session, "cart");
                if(cart != null)
                {
                    ViewBag.cart = cart;
                    ViewBag.price = FirstTotalPrice(cart);
                    ViewBag.tax = Tax(ViewBag.price);
                    ViewBag.total = TotalCost(ViewBag.price, ViewBag.tax);
                }
                return View();
            }
        }

      
        private bool CartExists(int id)
        {
            return _context.Cart.Any(e => e.CartId == id);
        }

        /*Cart actions: Remove Product, Add Product, finding Product */
        public IActionResult Remove(int id)
        {
            List<ItemsForCart> cart = SessionHelper.GetObjectFromJson<List<ItemsForCart>>(HttpContext.Session, "cart");
            cart = RemoveProduct(id, cart);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");
        }

        public IActionResult Buy(int id)
        {
            var product = _context.SellListings.FirstOrDefault(m => m.SellListingId == id);
            List<ItemsForCart> cart = SessionHelper.GetObjectFromJson<List<ItemsForCart>>(HttpContext.Session, "cart");

            if(cart == null)
            {
                cart = AddToCart(product, null);
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                cart = BuyProduct(id, cart, product);
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("Index");
        }

        //Checkout - THIS IS WHERE PAYMENT PORTAL
        //public async Task<IActionResult> CheckOut()
        public IActionResult CheckOut()
        {
            //TODO:
            /*
             * Figure out a solution for the paypal api
             * 
             */
            return View();
        }

        /*Cost Calculations for: Price, Tax, and total*/
        public double FirstTotalPrice(List<ItemsForCart> cart)
        {
            var totalPrice = cart.Sum(ItemsForCart => ItemsForCart.SellListing.SellPrice * ItemsForCart.SellListing.SellPrice);
            try
            {
                return (double)totalPrice;
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

        /*Cart actions calculations: Remove Product, Add Product, finding Product */

        public List<ItemsForCart> AddToCart(SellListing sellListing, List<ItemsForCart> existingCart)
        {
            List<ItemsForCart> cart = new List<ItemsForCart>();
            if (existingCart != null)
                cart = existingCart;

            cart.Add(new ItemsForCart
            {
                SellListing = sellListing,
                Quantity = 1
            });
            return cart;
        }

        public int FindIndex (int id, List<ItemsForCart> cart)
        {
            try
            {
                for (int i = 0; i < cart.Count; i++)
                {
                    if (cart[i].SellListing.SellListingId.Equals(id))
                        return i;
                }
                return -1;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception caught {0}", e);
                throw e;
            }
        }
        public List<ItemsForCart> BuyProduct(int id, List<ItemsForCart> cart, SellListing sellListing)
        {
            var index = FindIndex(id, cart);
            List<ItemsForCart> newCart;

            if (index != -1)
            {
                cart[index].Quantity++;
                newCart = cart;
            }
            else
                newCart = AddToCart(sellListing, cart);

            return newCart;
            
        }

        public List<ItemsForCart> RemoveProduct(int id, List<ItemsForCart> cart)
        {
            var index = FindIndex(id, cart);
            cart.RemoveAt(index);
            return cart;
        }
        
    }
}
