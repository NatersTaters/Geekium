using System;
using Xunit;
using Geekium.Controllers;
using Geekium.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace XUnitTestGeekium
{
	public class AccountTest
	{
		AccountViewModel accountViewModel;
		LoginViewModel loginViewModel;
		GeekiumContext context = new GeekiumContext();

		private void Initialize()
		{
			try { context.Entry(accountViewModel).State = EntityState.Detached; }
			catch (Exception) { }

			accountViewModel = new AccountViewModel()
			{
				Username = "Username",
				Password = "Password",
				FirstName = "Nathaniel",
				LastName = "Saunders",
				Email = "email@gmail.com"
			};

			try { context.Entry(loginViewModel).State = EntityState.Detached; }
			catch (Exception) { }

			loginViewModel = new LoginViewModel()
			{
				Username = "Username",
				Password = "Password"
			};
		}

		//[Fact]
		//public void ValidAccountControllerCreate_ShouldAllowCreate()
		//{
		//	//Arrange
		//	Initialize();
		//	var accountsController = new AccountsController(context);

		//	//Act
		//	accountsController.Create(accountViewModel);

		//	//Assert
		//	Assert.True(accountsController.ModelState.IsValid);
		//}

		//[Fact]
		//public void InvalidAccountControllerCreate_ShouldThrowError()
		//{
		//	//Arrange
		//	Initialize();
		//	var accountsController = new AccountsController(context);
		//	accountsController.ModelState.AddModelError("test", "test");

		//	//Act
		//	accountsController.Create(model: null);

		//	//Assert
		//	Assert.False(accountsController.ModelState.IsValid);
		//}

		//[Fact]
		//public void ValidAccountControllerLogin_ShouldAllowLogin()
		//{
		//	//Arrange
		//	Initialize();
		//	var accountsController = new AccountsController(context);

		//	//Act
		//	accountsController.Login(loginViewModel);

		//	//Assert
		//	Assert.True(accountsController.ModelState.IsValid);
		//}

		//[Fact]
		//public void InvalidAccountControllerLogin_ShouldThrowError()
		//{
		//	//Arrange
		//	Initialize();
		//	var accountsController = new AccountsController(context);
		//	accountsController.ModelState.AddModelError("test", "test");

		//	//Act
		//	accountsController.Login(loginViewModel);

		//	//Assert
		//	Assert.False(accountsController.ModelState.IsValid);
		//}

		[Fact]
		public void ValidAccountViewModel_ShouldNotThrowError()
		{
			//Arrange
			Initialize();

			//Act
			var validationContext = new ValidationContext(accountViewModel, null, null);
			var results = new List<ValidationResult>();
			var isModelStateValid = Validator.TryValidateObject(accountViewModel, validationContext, results, true);

			//Assert
			Assert.True(isModelStateValid);
		}

		[Fact]
		public void InvalidAccountViewModel_ShouldThrowError()
		{
			//Arrange
			Initialize();
			accountViewModel.Email = "";

			//Act
			var validationContext = new ValidationContext(accountViewModel, null, null);
			var results = new List<ValidationResult>();
			var isModelStateValid = Validator.TryValidateObject(accountViewModel, validationContext, results, true);

			//Assert
			Assert.False(isModelStateValid);
		}

		[Fact]
		public void ValidLoginViewModel_ShouldNotThrowError()
		{
			//Arrange
			Initialize();

			//Act
			var validationContext = new ValidationContext(loginViewModel, null, null);
			var results = new List<ValidationResult>();
			var isModelStateValid = Validator.TryValidateObject(loginViewModel, validationContext, results, true);

			//Assert
			Assert.True(isModelStateValid);
		}

		[Fact]
		public void InvalidLoginViewModel_ShouldThrowError()
		{
			//Arrange
			Initialize();
			loginViewModel.Username = "";

			//Act
			var validationContext = new ValidationContext(loginViewModel, null, null);
			var results = new List<ValidationResult>();
			var isModelStateValid = Validator.TryValidateObject(loginViewModel, validationContext, results, true);

			//Assert
			Assert.False(isModelStateValid);
		}
	}
}
