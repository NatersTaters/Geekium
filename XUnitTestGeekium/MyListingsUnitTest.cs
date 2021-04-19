using Geekium.Controllers;
using Geekium.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Hosting;
using System.Threading;
namespace XUnitTestGeekium
{
    public class MyListingsUnitTest
    {
        GeekiumContext context = new GeekiumContext();
        private readonly IWebHostEnvironment _hostEnvironment;

        #region Initialize
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

        void InitializePriceTrend()
        {
            PriceTrend priceTrend = new PriceTrend
            {
                ItemName = "PS5",
                DateOfUpdate = Convert.ToDateTime("2020-10-02"),
                AveragePrice = 500,
                HighestPrice = 9999,
                LowestPrice = 1
            };
            context.Add(priceTrend);
        }

        public SellListing InitializeSellListing()
        {
            InitializePriceTrend();
            SellListing listing = new SellListing
            {
                SellerId = 1,
                PriceTrendId = 1,
                SellTitle = "Sell 1",
                SellPrice = 99,
                SellDate = Convert.ToDateTime("2020-10-02"),
                SellItemType = "Game",
                SellQuantity = 3,
                PriceTrendKeywords = "PS5",
            };
            return listing;
        }

        public TradeListing InitializeTradeListing()
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
            return listing;
        }

        public ServiceListing InitializeServiceListing()
        {
            context.Add(InitializeAccount());
            ServiceListing listing = new ServiceListing
            {
                AccountId = 1,
                ServiceTitle = "Nothing",
                ServiceDescription = "Literally nothing",
                ListingDate = Convert.ToDateTime("2020-10-02"),
                ServiceImage = null
            };

            return listing;
        }
        #endregion

        // Call CityStateCountByIp
        // Pass in a known ip address
        // Return "info" of type IpInfo
        [Fact]
        public async void CityStateCountByIp_PassInIp_ReturnIpInfo()
        {
            // Arrange
            MyListingsController controller = new MyListingsController(context, _hostEnvironment);

            // Act
            string ip = "99.226.48.14";
            var info = await controller.CityStateCountByIp(ip);

            // Assert
            Assert.IsType<IpData.Models.IpInfo>(info);
        }

        // Call PopulateDropdown
        // Returns a list of select list items
        [Fact]
        public void PopulateDropdown_SendYes_ReturnListOfSelectListItems()
        {
            // Arrange
            MyListingsController controller = new MyListingsController(context, _hostEnvironment);

            // Act
            var list = controller.PopulateDropdown();

            // Assert
            Assert.True(list.Count > 0);
        }

        // Call Index()
        // View should be returned
        [Fact]
        public async void Index_CallFunction_ReturnView()
        {
            // Arrange
            MyListingsController controller = new MyListingsController(context, _hostEnvironment);
            // Act
            var actionResult = await controller.Index();

            // Assert
            Assert.IsType<ViewResult>(actionResult);
        }

        #region Sell Listings
        // Call CreateSell()
        // Pass the initialized sell listing
        // Model returns okay
        [Fact]
        public async void CreateSell_InputViableSellData_ModelReturnsValid()
        {
            // Arrange
            //GenerateSession();
            MyListingsController controller = new MyListingsController(context, _hostEnvironment);
            SellListing list = InitializeSellListing();

            // Act
            await controller.CreateSell(list);

            // Assert
            Assert.True(controller.ModelState.IsValid);
        }

        // Call SellDetails()
        // Pass in null
        // Result should be okay
        [Fact]
        public async Task SellDetails_PassInNull_ReturnNotFound()
        {
            // Arrange
            MyListingsController controller = new MyListingsController(context, _hostEnvironment);
           
            // Act
            var actionResult = await controller.SellDetails(null);

            // Assert
            Assert.IsType<NotFoundResult>(actionResult);
        }

        // Call DeleteConfirmedSelling
        // Pass in the sell listing id of 0
        // Get tot the redirection view
        [Fact]
        public async Task DeleteConfirmedSelling_PassInInitializedSellListingId_ReturnRedirectionToActionResult ()
        {
            // Assert
            MyListingsController controller = new MyListingsController(context, _hostEnvironment);
            SellListing list = InitializeSellListing();
            InitializeSeller();

            // Act
            context.Add(list);
            var result = await controller.DeleteConfirmedSelling(0);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
        }

        // Call EditSell
        // Pass in a sell listing and modify it
        // Return valid model state
        [Fact]
        public async Task EditSell_PassSellIdAndModifiedSellListing_ModelIsValid()
        {
            // Assert
            MyListingsController controller = new MyListingsController(context, _hostEnvironment);
            SellListing list = InitializeSellListing();

            // Act
            context.Add(list);
            list.SellPrice = 555555;
            var result = await controller.EditSell(list.SellListingId, list);

            // Assert
            Assert.True(controller.ModelState.IsValid);
            Assert.IsType<ViewResult>(result);
        }

        #endregion

        #region Trade Listings
        // Call CreateTrade()
        // Pass the initialized trade listing
        // Model returns okay
        [Fact]
        public async void CreateTrade_InputViableTradeData_ModelReturnsValid()
        {
            // Arrange
            MyListingsController controller = new MyListingsController(context, _hostEnvironment);
            TradeListing list = InitializeTradeListing();

            // Act
            await controller.CreateTrade(list);

            // Assert
            Assert.True(controller.ModelState.IsValid);
        }

        // Call TradeDetails()
        // Pass in null
        // No result found error should be returned
        [Fact]
        public async Task TradeDetails_PassInNull_ReturnNotFound()
        {
            // Arrange
            MyListingsController controller = new MyListingsController(context, _hostEnvironment);

            // Act
            var actionResult = await controller.TradeDetails(null);

            // Assert
            Assert.IsType<NotFoundResult>(actionResult);
        }

        // Call DeleteConfirmedTrade
        // Pass in the trade listing id of 0
        // Get tot the redirection view
        [Fact]
        public async Task DeleteConfirmedTrade_PassInInitializedTradeId_ReturnRedirection()
        {
            // Assert
            MyListingsController controller = new MyListingsController(context, _hostEnvironment);
            TradeListing list = InitializeTradeListing();
            InitializeSeller();

            // Act
            context.Add(list);
            var result = await controller.DeleteConfirmedSelling(0);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
        }

        // Call EditTrade
        // Send in Trade Listing and Trade Listing ID
        // Return valid model state
        [Fact]
        public async Task EditTrade_PassInTradeListingIdAndTradeListing_ModelIsValid()
        {
            // Assert
            MyListingsController controller = new MyListingsController(context, _hostEnvironment);
            TradeListing list = InitializeTradeListing();

            // Act
            context.Add(list);
            list.TradeDescription = "Hello";
            await controller.EditTrade(0, list);

            // Assert
            Assert.True(controller.ModelState.IsValid);
        }


        #endregion

        #region Service Listings
        // Call CreateService()
        // Pass the initialized service listing
        // Model returns okay
        [Fact]
        public async void CreateService_InputViableServiceData_ModelReturnsValid()
        {
            // Arrange
            MyListingsController controller = new MyListingsController(context, _hostEnvironment);
            ServiceListing list = InitializeServiceListing();

            // Act
            await controller.CreateService(list);

            // Assert
            Assert.True(controller.ModelState.IsValid);
        }


        // Call ServiceDetails()
        // Pass in null
        // No result found error should be returned
        [Fact]
        public async Task ServiceDetails_PassInNull_ReturnNotFound()
        {
            // Arrange
            MyListingsController controller = new MyListingsController(context, _hostEnvironment);

            // Act
            var actionResult = await controller.ServiceDetails(null);

            // Assert
            Assert.IsType<NotFoundResult>(actionResult);
        }

        // Call DeleteConfirmedTrade
        // Pass in the service listing id of 0
        // Get tot the redirection view
        [Fact]
        public async Task DeleteConfirmedService_PassInInitializedServiceId_ReturnRedirection()
        {
            // Assert
            MyListingsController controller = new MyListingsController(context, _hostEnvironment);
            ServiceListing list = InitializeServiceListing();
            InitializeSeller();

            // Act
            context.Add(list);
            var result = await controller.DeleteConfirmedService(0);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
        }

        // Call EditService
        // Do not send in a model
        // Return valid model state
        [Fact]
        public async Task EditService_PassInServiceListingAndServiceListingId_ModelIsValid()
        {
            // Assert
            MyListingsController controller = new MyListingsController(context, _hostEnvironment);
            ServiceListing list = InitializeServiceListing();

            // Act
            context.Add(list);
            list.ServiceDescription = "Hello";
            await controller.EditService(0, list);

            // Assert
            Assert.True(controller.ModelState.IsValid);
        }

        #endregion

    }
}
