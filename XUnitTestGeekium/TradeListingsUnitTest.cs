using Geekium.Controllers;
using Geekium.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestGeekium
{
    public class TradeListingsUnitTest
    {
        GeekiumContext context = new GeekiumContext();

        #region Initialize
        Account InitializeAccount()
        {
            Account account = new Account
            {
                UserName = "Hello",
                UserPassword = "ABC",
                PaswordHash = "ASDASDADS",
                FirstName = "First",
                LastName = "Last",
                Email = "email@email.com",
                PointBalance = 0
            };

            return account;
        }

        TradeListing InitializeTrade()
        {
            context.Add(InitializeAccount());
            TradeListing listing = new TradeListing
            {
                Seller = InitializeSeller(),
                SellerId = 1,
                TradeTitle = "Trade 1",
                TradeDescription = "This is the first trade",
                TradeFor = "Water",
                TradeDate = Convert.ToDateTime("2020-10-02"),
                TradeItemType = "Game",
                TradeQuantity = 3,
                TradeImage = null,
                TradeLocation = "Ontario"
            };
            context.Add(listing);
            return listing;
        }

        SellerAccount InitializeSeller()
        {
            SellerAccount seller = new SellerAccount
            {
                AccountId = 0,
                AverageRating = 0,
                Account = InitializeAccount()
            };

            context.Add(seller);
            return seller;
        }
        #endregion

        // Call Index()
        // View should be returned
        [Fact]
        public async void Index_CallFunction_ReturnView()
        {
            // Arrange
            TradeListingsController controller = new TradeListingsController(context);

            // Act
            var actionResult = await controller.Index();

            // Assert
            Assert.IsType<ViewResult>(actionResult);
        }

        // Call Details()
        // Pass in null
        // No result found error should be returned
        [Fact]
        public void Details_PassInNull_ReturnNotFound()
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
        // Return view result
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
        // Return view result anyway
        [Fact]
        public async Task FilterTrades_SendNullData_ReturnRedirect()
        {
            // Arrange
            TradeListingsController controller = new TradeListingsController(context);

            // Act
            var actionResult = await controller.FilterTrades(null);

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
            TradeListingsController controller = new TradeListingsController(context);

            // Act
            var list = controller.PopulateDropdown("Yes");

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
