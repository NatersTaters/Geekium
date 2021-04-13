using Geekium.Controllers;
using Geekium.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestGeekium
{
    public class SellListingsUnitTest
    {
        GeekiumContext context = new GeekiumContext();

        // Call Details()
        // Pass in null
        // No result found error should be returned
        [Fact]
        public void SellDetails_PassInNull_ReturnNotFound()
        {
            // Arrange
            SellListingsController controller = new SellListingsController(context);

            // Act
            var actionResult = controller.Details(null);

            // Assert
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        // Call FilterProducts()
        // Pass in "Hello", 3 and 200
        // Return view
        //[Fact]
        //public async Task FilterProducts_SendViableValues_ReturnView()
        //{
        //    // Arrange
        //    SellListingsController controller = new SellListingsController(context);

        //    // Act
        //    var actionResult = await controller.FilterProducts("Hello", 3, 200);

        //    // Assert
        //    Assert.IsType<ViewResult>(actionResult);
        //}

        // Call FilterProducts()
        // Pass in null, 0, 0
        // Return redirection
        //[Fact]
        //public async Task FilterProducts_SendNullData_ReturnRedirect()
        //{
        //    // Arrange
        //    SellListingsController controller = new SellListingsController(context);

        //    // Act
        //    var actionResult = await controller.FilterProducts(null, 0, 0);

        //    // Assert
        //    Assert.IsType<RedirectToActionResult>(actionResult);
        //}
    }
}
