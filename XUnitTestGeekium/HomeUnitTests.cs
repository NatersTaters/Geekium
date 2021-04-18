using Geekium.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Geekium.Controllers;

namespace XUnitTestGeekium
{
	public class HomeUnitTests
	{
		GeekiumContext context = new GeekiumContext();
		ILogger<HomeController> logger;
		Support support;

		private void Initialize()
		{
			try { context.Entry(support).State = EntityState.Detached; }
			catch (Exception) { }

			support = new Support()
			{
				SupportHeader = "Support Header",
				SupportBody = "Support Body"
			};
		}

		[Fact]
		public void ValidSupport_ShouldAccept()
		{
			//Assert
			Initialize();
			var homeController = new HomeController(logger, context);

			//Act
			homeController.SendSupport(support);

			//Assert
			Assert.True(homeController.ModelState.IsValid);
		}

		[Fact]
		public void InvalidSupport_ShouldThrowError()
		{
			//Assert
			Initialize();
			var homeController = new HomeController(logger, context);
			homeController.ModelState.AddModelError("test", "test");

			//Act
			homeController.SendSupport(support: null);

			//Assert
			Assert.False(homeController.ModelState.IsValid);
		}
	}
}
