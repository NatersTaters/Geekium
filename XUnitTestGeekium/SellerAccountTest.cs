using System;
using Xunit;
using Geekium.Controllers;
using Geekium.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace XUnitTestGeekium
{
	public class SellerAccountTest
	{
		UpgradeViewModel upgradeViewModel;
		GeekiumContext context = new GeekiumContext();

		private void Initialize()
		{
			try { context.Entry(upgradeViewModel).State = EntityState.Detached; }
			catch (Exception) { }

			upgradeViewModel = new UpgradeViewModel()
			{
				Code = "123456"
			};
		}

		//[Fact]
		//public void ValidAccountControllerUpgrade_ShouldAllowUpgrade()
		//{
		//	//Arrange
		//	Initialize();
		//	var accountsController = new AccountsController(context);

		//	//Act
		//	accountsController.Upgrade(upgradeViewModel);

		//	//Assert
		//	Assert.True(accountsController.ModelState.IsValid);
		//}

		//[Fact]
		//public void InvalidAccountControllerUpgrade_ShouldThrowError()
		//{
		//	Arrange
		//	Initialize();
		//	var accountsController = new AccountsController(context);
		//	accountsController.ModelState.AddModelError("test", "test");

		//	Act
		//	accountsController.Upgrade(model: null);

		//	Assert
		//	Assert.False(accountsController.ModelState.IsValid);
		//}

		[Fact]
		public void ValidUpgradeViewModel_ShouldNotThrowError()
		{
			//Arrange
			Initialize();

			//Act
			var validationContext = new ValidationContext(upgradeViewModel, null, null);
			var results = new List<ValidationResult>();
			var isModelStateValid = Validator.TryValidateObject(upgradeViewModel, validationContext, results, true);

			//Assert
			Assert.True(isModelStateValid);
		}

		[Fact]
		public void InvalidUpgradeViewModel_ShouldThrowError()
		{
			//Arrange
			Initialize();
			upgradeViewModel.Code = "";

			//Act
			var validationContext = new ValidationContext(upgradeViewModel, null, null);
			var results = new List<ValidationResult>();
			var isModelStateValid = Validator.TryValidateObject(upgradeViewModel, validationContext, results, true);

			//Assert
			Assert.False(isModelStateValid);
		}
	}
}
