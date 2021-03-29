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
    public class ServiceListingsUnitTest
    {
        GeekiumContext context = new GeekiumContext();

        // Call details()
        // Pass in null
        // No result found error should be returned
        [Fact]
        public void ServiceDetails_PassInNull_ReturnNotFound()
        {
            // Arrange
            ServiceListingsController controller = new ServiceListingsController(context);

            // Act
            var actionResult = controller.Details(null);

            // Assert
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        // Call FilterServices()
        // Pass in "Hello"
        // Return view
        [Fact]
        public async Task FilterServices_SendViableValues_ReturnView()
        {
            // Arrange
            ServiceListingsController controller = new ServiceListingsController(context);

            // Act
            var actionResult = await controller.FilterServices("Hello");

            // Assert
            Assert.IsType<ViewResult>(actionResult);
        }

        // Call FilterServices()
        // Pass in null
        // Return redirection
        [Fact]
        public async Task FilterServices_SendNullData_ReturnRedirect()
        {
            // Arrange
            ServiceListingsController controller = new ServiceListingsController(context);

            // Act
            var actionResult = await controller.FilterServices(null);

            // Assert
            Assert.IsType<RedirectToActionResult>(actionResult);
        }
    }
}
