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

        // Call Index()
        // View should be returned
        [Fact]
        public async void Index_CallFunction_ReturnView()
        {
            // Arrange
            SellListingsController controller = new SellListingsController(context);

            // Act
            var actionResult = await controller.Index();

            // Assert
            Assert.IsType<ViewResult>(actionResult);
        }

        // Call MerchandiseIndex()
        // View should be returned
        [Fact]
        public async void MerchandiseIndex_CallFunction_ReturnView()
        {
            // Arrange
            SellListingsController controller = new SellListingsController(context);

            // Act
            var actionResult = await controller.MerchandiseIndex();

            // Assert
            Assert.IsType<ViewResult>(actionResult);
        }


        // Call Details()
        // Pass in 2
        // No result found error should be returned
        [Fact]
        public void ChooseIndex_PassIn2_ReturnRedirection()
        {
            // Arrange
            SellListingsController controller = new SellListingsController(context);

            // Act
            var actionResult = controller.ChooseIndex(2);

            // Assert
            Assert.IsType<RedirectToActionResult>(actionResult.Result);
        }
        // Call Details
        // Pass in null
        // Return not found view
        [Fact]
        public void Details_PassInNull_ReturnNotFound()
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
        [Fact]
        public async Task FilterProducts_SendViableValues_ReturnView()
        {
            // Arrange
            SellListingsController controller = new SellListingsController(context);

            // Act
            var actionResult = await controller.FilterProducts("Hello", 3, 200);

            // Assert
            Assert.IsType<ViewResult>(actionResult);
        }

        // Call FilterMerchandise()
        // Pass in null, 0, 0
        // Return redirection
        [Fact]
        public async Task FilterMerchandise_SendNullData_ReturnRedirect()
        {
            // Arrange
            SellListingsController controller = new SellListingsController(context);

            // Act
            var actionResult = await controller.FilterMerchandise(null, 0, 0);

            // Assert
            Assert.IsType<ViewResult>(actionResult);
        }

        // Call PopulateDropdown
        // Pass in "Yes"
        // Returns a list of select list items
        [Fact]
        public void PopulateDropdown_SendYes_ReturnListOfSelectListItems()
        {
            // Arrange
            SellListingsController controller = new SellListingsController(context);

            // Act
            var list = controller.PopulateDropdown("Yes");

            // Assert
            Assert.True(list.Count > 0);
        }

        // Call SetViewBag
        // Pass in "Hello", 0, 0 and ""
        // The collapse viewbag should be populated with "collapse"
        // The search viewbag should be populated with "Hello"
        [Fact]
        public void SetViewBag_PassInHelloAndNothing_ReturnCollapseAndHelloInViewBags()
        {
            // Arrange
            SellListingsController controller = new SellListingsController(context);

            // Act
            controller.SetViewBag("Hello", 0, 0, "");

            // Assert
            Assert.Equal("collapse", controller.ViewBag.Collapse);
            Assert.Equal("Hello", controller.ViewBag.Search);
        }
    }
}
