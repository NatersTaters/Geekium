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

        // Call Index()
        // View should be returned
        [Fact]
        public async void Index_CallFunction_ReturnView()
        {
            // Arrange
            ServiceListingsController controller = new ServiceListingsController(context);

            // Act
            var actionResult = await controller.Index();

            // Assert
            Assert.IsType<ViewResult>(actionResult);
        }

        // Call details()
        // Pass in null
        // No result found error should be returned
        [Fact]
        public void Details_PassInNull_ReturnNotFound()
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
        // Return view
        [Fact]
        public async Task FilterServices_SendNullData_ReturnRedirect()
        {
            // Arrange
            ServiceListingsController controller = new ServiceListingsController(context);

            // Act
            var actionResult = await controller.FilterServices(null);

            // Assert
            Assert.IsType<ViewResult>(actionResult);
        }

        // Call PopulateDropdown
        // Pass in null
        // Returns a list of select list items
        [Fact]
        public void PopulateDropdown_SendNull_ReturnListOfSelectListItems()
        {
            // Arrange
            ServiceListingsController controller = new ServiceListingsController(context);

            // Act
            var list = controller.PopulateDropdown(null);

            // Assert
            Assert.True(list.Count > 0);
        }

        // Call SetViewBag
        // Pass in "Hello" and ""
        // The collapse viewbag should be populated with "collapse"
        // The search viewbag should be populated with "Hello"
        [Fact]
        public void SetViewBag_PassInHelloAndNothing_ReturnCollapseAndHelloInViewBags()
        {
            // Arrange
            TradeListingsController controller = new TradeListingsController(context);

            // Act
            controller.SetViewBag("Hello", "");

            // Assert
            Assert.Equal("collapse", controller.ViewBag.Collapse);
            Assert.Equal("Hello", controller.ViewBag.Search);
        }
    }
}
