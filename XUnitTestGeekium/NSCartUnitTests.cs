using Geekium.Controllers;
using Geekium.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestGeekium
{
	public class NSCartUnitTests
	{
		GeekiumContext context = new GeekiumContext();
		IWebHostEnvironment hostEnvironment;

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
	}
}
