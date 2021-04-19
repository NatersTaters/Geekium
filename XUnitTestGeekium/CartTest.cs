using Geekium.Controllers;
using Geekium.Models;
using Stripe;
using System;
using System.Collections.Generic;
using Xunit;

namespace XUnitTestGeekium
{
    public class CartTest
    {

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
        //Check if the taxes are being calculated correctly
        [Fact]
        public void ProductTax_ShouldReturn195()
        {
            // Arrange 
            CartsController context = new CartsController();
            double subTotal = 1500;
            double tax = 195;

            // Act
            double total = context.Tax(subTotal);

            // Assert
            Assert.Equal(total, tax);
        }
        [Fact]
        public void ProductTax_NegativeValuesShouldStillCalculateAndPass()
        {
            // Arrange 
            CartsController context = new CartsController();
            double subTotal = -1500;
            double tax = -195;

            // Act
            double total = context.Tax(subTotal);

            //Assert
            Assert.Equal(total, tax);
        }

        //Check return method of total price to make sure it returns the right total price
        [Fact]
        public void ProductTotalPrice_ShouldReturn1695()
        {
            // Arrange
            CartsController context = new CartsController();
            double subTotal = 1500;
            double tax = 195;
            double total = subTotal + tax;


            // Act
            double calculatedTotal = context.TotalCost(subTotal, tax);

            // Assert
            Assert.Equal(total, calculatedTotal);
        }
        //Total should still be retrieved even with no tax
        [Fact]
        public void ProductTotalIfTax0_ShouldStillReturn()
        {
            // Arrange
            CartsController context = new CartsController();
            double subTotal = 1500;
            double tax = 0;
            double total = subTotal + tax;

            // Act
            double calculatedTotal = context.TotalCost(subTotal, tax);

            // Arrange
            Assert.Equal(total, calculatedTotal);
        }
        //Cart Total of 21.47, should retrieve 215 points
        [Fact]
        public void TotalPointsOfPurchase21Dollars47Cents_ShouldReturn215Points()
        {
            // Arrange
            CartsController context = new CartsController();
            double cartTotal = 21.47;
            double pointsShouldRetrieve = 215;

            // Act
            double realPointValue = context.PointsEarned(cartTotal);

            // Arrange
            Assert.Equal(pointsShouldRetrieve, realPointValue);
            
        }
        [Fact]
        public void TotalPointsOfPurchaseIfNegativeValues_ShouldStillFunctionAndNotCauseErrors()
        {
            // Arrange
            CartsController context = new CartsController();
            double cartTotal = -21.47;
            double pointsShouldRetrieve = -215;

            // Act
            double realPointValue = context.PointsEarned(cartTotal);


            // Arrange
            Assert.Equal(pointsShouldRetrieve, realPointValue);
        }
        [Fact]
        public void TotalPointsOfPurchaseIfNull_ShouldFail()
        {
            // Arrange
            CartsController context = new CartsController();
            double? cartTotal;
            cartTotal = null;
            double pointsShouldRetrieve = 0;

            // Act
            double realPointValue = context.PointsEarned((double)cartTotal);

            // Arrange
            Assert.Equal(pointsShouldRetrieve, realPointValue);
        }
        [Fact]
        public void TotalStripeAmount_ShouldConvertTheTotalProperlyForStripe()
        {
            // Arrange
            CartsController context = new CartsController();
            double total = 1695;
            double stripeExpectedTotal = total * 100;


            // Act
            double calculatedTotal = context.StripeAmount(total);

            // Assert
            Assert.Equal(stripeExpectedTotal, calculatedTotal);
        }
        //Stripe Tax rounded should return 390 rounded from 389.61
        [Fact]
        public void StripeTaxAmount_ShouldReturnTaxAsProperConversion()
        {
            // Arrange
            CartsController context = new CartsController();
            double subTotal = 2997;
            double tax = 0.13;
            double expectedTaxValue = 390;

            // Act
            double outcomeTaxValue = context.StripeTax(subTotal);

            // Assert
            Assert.Equal(expectedTaxValue, outcomeTaxValue);
        }
        // Check if the charge is going through
        // 
        [Fact]
        public async void StripeCheckoutProcess_ShouldReturnTrueIfCheckoutSucceeded()
        {
            StripeConfiguration.ApiKey = "sk_test_51IXwmnAzF1FhVDs7TrRn2e6FSkWrldNuZ1rBH1IeR9Zu1OozRj4PgojaAJYmtC0RRPZ6zZJwQqerwKAJFhG8V7Jp00jtpTJewD";

            // Arrange 
            CartsController context = new CartsController();
            List<ItemsForCart> cart = Cart();
            double sAmount = 1500;
            double tax = 195;

            long amount = (long)context.TotalCost(sAmount, tax);
            string stripeEmail = "email@gmail.com";
            bool charged = false;
            string stripeToken = "tok_visa"; //tok_1IhjCiAzF1FhVDs7NdHCHmeB

            // Act
            await context.CheckOut(stripeEmail, stripeToken, charged);

            // Assert
            Assert.True(charged);
        }

    }
}
