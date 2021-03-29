using Geekium.Controllers;
using Geekium.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace XUnitTestGeekium
{
    public class LGUnitTests
    {
        //CartsController context;

        public SellListing Product()
        {   //Initializing a sample selllisting to use in use cases 
            SellListing sellListing = new SellListing
            {
                SellListingId = 21,
                SellTitle = "PS5",
                SellDescription = "PS5 because it was restocked",
                SellPrice = 500,
                SellQuantity = 3,
            };
            return sellListing;
        }
        //Creating the cart, adding sample listing to cart
        public List<ItemsForCart> Cart()
        {
            SellListing sellListing = Product();

            ItemsForCart product = new ItemsForCart
            {
                SellListing = sellListing,
                Quantity = 3
            };

            List<ItemsForCart> cart = new List<ItemsForCart>();
            cart.Add(product);

            return cart;
        }
        //Check the total of the products in cart
        //Since there are 3 products the cart should have a total of 1500
        [Fact]
        public void SellListingSum_ReturnProperTotalOf1500()
        {
            // Arrange 
            CartsController context = new CartsController();
            List<ItemsForCart> cart = Cart();
            double totalOfProducts = 1500;

            // Act
            double total = context.FirstTotalPrice(cart);

            // Assert
            Assert.Equal(totalOfProducts, total);
        }
        //Check if the taxes are being calculated correctly
        [Fact]
        public void CheckProperTaxationFor1500_ShouldBe195()
        {
            // Arrange
            CartsController context = new CartsController();
            List<ItemsForCart> cart = Cart();
            double taxAmount = 195;

            // Act
            double tax = context.Tax(1500);

            // Assert
            Assert.Equal(taxAmount, tax);
        }
        //Products removed from cart 
        //Cart will be empty, check to make sure the cart is empty
        [Fact]
        public void RemoveProductFromTheCart()
        {
            // Arrange
            CartsController context = new CartsController();
            List<ItemsForCart> cart = Cart();
            SellListing sellListing = Product();
            int emptyCart = 0;

            // Act
            cart = context.RemoveProduct(sellListing.SellListingId, cart);
            int productsInCart = cart.Count;

            // Assert
            Assert.Equal(emptyCart, productsInCart);
        }
        // Check to make sure the total summed price is being calculated properly
        // With tax included, the price of the 3 PS5s should be 1500 + 195
        // This should total 1695
        [Fact]
        public void AddedTotalBetweenPriceAndTax_ShouldBe1695()
        {
            // Arrange 
            CartsController context = new CartsController();
            List<ItemsForCart> cart = Cart();
            double totalPrice = 1695;

            // Act
            double price = context.TotalCost(1500, 195);

            // Assert
            Assert.Equal(totalPrice, price);
        }

    }
}
