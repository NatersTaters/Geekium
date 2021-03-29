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
    public class TradeListingsUnitTest
    {
        GeekiumContext context = new GeekiumContext();

        // Call Details()
        // Pass in null
        // No result found error should be returned
        [Fact]
        public void TradeDetails_PassInNull_ReturnNotFound()
        {
            // Arrange
            TradeListingsController controller = new TradeListingsController(context);

            // Act
            var actionResult = controller.Details(null);

            // Assert
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        // Call FilterTrades()
        // Pass in "Hello"
        // Return view
        [Fact]
        public async Task FilterTrades_SendViableValues_ReturnView()
        {
            // Arrange
            TradeListingsController controller = new TradeListingsController(context);

            // Act
            var actionResult = await controller.FilterTrades("Hello");

            // Assert
            Assert.IsType<ViewResult>(actionResult);
        }

        // Call FilterTrades()
        // Pass in null
        // Return redirection
        [Fact]
        public async Task FilterTrades_SendNullData_ReturnRedirect()
        {
            // Arrange
            TradeListingsController controller = new TradeListingsController(context);

            // Act
            var actionResult = await controller.FilterTrades(null);

            // Assert
            Assert.IsType<RedirectToActionResult>(actionResult);
        }
    }
}
