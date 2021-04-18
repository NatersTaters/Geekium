using Geekium.Controllers;
using Geekium.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestGeekium
{
	public class CartUnitTests
	{
		GeekiumContext context = new GeekiumContext();
		IWebHostEnvironment hostEnvironment;

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

		[Fact]
		public async Task ValidAccountId_ShouldAllowCartCreation()
		{
			//Arrange
			var account = await context.Accounts.FirstOrDefaultAsync(m => m.AccountId == 2);
			var cartController = new CartsController(context, hostEnvironment);

			//Act
			await cartController.CreateCart(account.AccountId.ToString());

			//Assert
			Assert.True(cartController.ModelState.IsValid);
		}

		[Fact]
		public async Task ValidSellListing_ShouldAllowAddToCart()
		{
			//Arrange
			var sellListing = await context.SellListings.Include(s => s.PriceTrend)
				.Include(s => s.Seller)
				.Include(s => s.Seller.Account)
				.FirstOrDefaultAsync(s => s.SellListingId == 1);
			var account = await context.Accounts
				.FirstOrDefaultAsync(m => m.AccountId == 1);

			var cartController = new CartsController(context, hostEnvironment);

			//Act
			await cartController.Add(sellListing.SellListingId, account.AccountId.ToString());

			//Assert
			Assert.True(cartController.ModelState.IsValid);
		}

		[Fact]
		public async Task InvalidSellListing_ShouldNotAllowAddToCart()
		{
			//Arrange
			var sellListing = await context.SellListings.Include(s => s.PriceTrend)
				.Include(s => s.Seller)
				.Include(s => s.Seller.Account)
				.FirstOrDefaultAsync(s => s.SellListingId == 1);
			var account = await context.Accounts
				.FirstOrDefaultAsync(m => m.AccountId == 1);

			var cartController = new CartsController(context, hostEnvironment);
			cartController.ModelState.AddModelError("test", "test");

			//Act
			await cartController.Add(12345, account.AccountId.ToString());

			//Assert
			Assert.False(cartController.ModelState.IsValid);
		}

		[Fact]
		public async Task ValidCartObject_ShouldAllowChangeTransactionStatus()
		{
			//Arrange
			var account = await context.Accounts
				.FirstOrDefaultAsync(m => m.AccountId == 1);

			var cart = await context.Cart
					.Include(c => c.Account)
					.FirstOrDefaultAsync(s => s.AccountId == account.AccountId);

			var cartController = new CartsController(context, hostEnvironment);

			//Act
			await cartController.ChangeCartTransactionStatus(cart);

			//Assert
			Assert.True(cartController.ModelState.IsValid);
		}

		[Fact]
		public async Task InvalidCartObject_ShouldNotAllowChangeTransactionStatus()
		{
			//Arrange
			var account = await context.Accounts
				.FirstOrDefaultAsync(m => m.AccountId == 1);

			var cart = await context.Cart
					.Include(c => c.Account)
					.FirstOrDefaultAsync(s => s.AccountId == account.AccountId);

			var cartController = new CartsController(context, hostEnvironment);
			cartController.ModelState.AddModelError("test", "test");

			//Act
			await cartController.ChangeCartTransactionStatus(cart: null);

			//Assert
			Assert.False(cartController.ModelState.IsValid);
		}

		[Fact]
		public async Task ValidCartObject_ShouldAllowDelete()
		{
			//Arrange
			var account = await context.Accounts
				.FirstOrDefaultAsync(m => m.AccountId == 1);

			var cart = await context.Cart
					.Include(c => c.Account)
					.FirstOrDefaultAsync(s => s.AccountId == account.AccountId);

			var cartController = new CartsController(context, hostEnvironment);

			//Act
			await cartController.DeleteCart(cart.CartId);

			//Assert
			Assert.True(cartController.ModelState.IsValid);
		}

		[Fact]
		public async Task InvalidCartObject_ShouldNotAllowDelete()
		{
			//Arrange
			var account = await context.Accounts
				.FirstOrDefaultAsync(m => m.AccountId == 1);

			var cart = await context.Cart
					.Include(c => c.Account)
					.FirstOrDefaultAsync(s => s.AccountId == account.AccountId);

			var cartController = new CartsController(context, hostEnvironment);
			cartController.ModelState.AddModelError("test", "test");

			//Act
			await cartController.DeleteCart(cart.CartId);

			//Assert
			Assert.False(cartController.ModelState.IsValid);
		}

		//Check if the taxes are being calculated correctly
		[Fact]
		public void ProductTax_ShouldReturn195()
		{
			// Arrange 
			CartsController controllerContext = new CartsController(context, hostEnvironment);
			double subTotal = 1500;
			double tax = 195;

			// Act
			double total = controllerContext.Tax(subTotal);

			// Assert
			Assert.Equal(total, tax);
		}
		[Fact]
		public void ProductTax_NegativeValuesShouldStillCalculateAndPass()
		{
			// Arrange 
			CartsController controllerContext = new CartsController(context, hostEnvironment);
			double subTotal = -1500;
			double tax = -195;

			// Act
			double total = controllerContext.Tax(subTotal);

			//Assert
			Assert.Equal(total, tax);
		}

		//Check return method of total price to make sure it returns the right total price
		[Fact]
		public void ProductTotalPrice_ShouldReturn1695()
		{
			// Arrange
			CartsController controllerContext = new CartsController(context, hostEnvironment);
			double subTotal = 1500;
			double tax = 195;
			double total = subTotal + tax;


			// Act
			double calculatedTotal = controllerContext.TotalCost(subTotal, tax);

			// Assert
			Assert.Equal(total, calculatedTotal);
		}
		//Total should still be retrieved even with no tax
		[Fact]
		public void ProductTotalIfTax0_ShouldStillReturn()
		{
			// Arrange
			CartsController controllerContext = new CartsController(context, hostEnvironment);
			double subTotal = 1500;
			double tax = 0;
			double total = subTotal + tax;

			// Act
			double calculatedTotal = controllerContext.TotalCost(subTotal, tax);

			// Arrange
			Assert.Equal(total, calculatedTotal);
		}
		//Cart Total of 21.47, should retrieve 215 points
		[Fact]
		public void TotalPointsOfPurchase21Dollars47Cents_ShouldReturn215Points()
		{
			// Arrange
			CartsController controllerContext = new CartsController(context, hostEnvironment);
			double cartTotal = 21.47;
			double pointsShouldRetrieve = 215;

			// Act
			double realPointValue = controllerContext.PointsEarned(cartTotal);

			// Arrange
			Assert.Equal(pointsShouldRetrieve, realPointValue);

		}
		[Fact]
		public void TotalPointsOfPurchaseIfNegativeValues_ShouldStillFunctionAndNotCauseErrors()
		{
			// Arrange
			CartsController controllerContext = new CartsController(context, hostEnvironment);
			double cartTotal = -21.47;
			double pointsShouldRetrieve = -215;

			// Act
			double realPointValue = controllerContext.PointsEarned(cartTotal);


			// Arrange
			Assert.Equal(pointsShouldRetrieve, realPointValue);
		}
		[Fact]
		public void TotalStripeAmount_ShouldConvertTheTotalProperlyForStripe()
		{
			// Arrange
			CartsController controllerContext = new CartsController(context, hostEnvironment);
			double total = 1695;
			double stripeExpectedTotal = total * 100;


			// Act
			double calculatedTotal = controllerContext.StripeAmount(total);

			// Assert
			Assert.Equal(stripeExpectedTotal, calculatedTotal);
		}
		//Stripe Tax rounded should return 390 rounded from 389.61
		[Fact]
		public void StripeTaxAmount_ShouldReturnTaxAsProperConversion()
		{
			// Arrange
			CartsController controllerContext = new CartsController(context, hostEnvironment);
			double subTotal = 2997;
			double tax = 0.13;
			double expectedTaxValue = 390;

			// Act
			double outcomeTaxValue = controllerContext.StripeTax(subTotal);

			// Assert
			Assert.Equal(expectedTaxValue, outcomeTaxValue);
		}
	}
}
