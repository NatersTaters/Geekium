using Geekium.Controllers;
using Geekium.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Geekium.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Net.Mail;
using System.Net;

namespace XUnitTestGeekium
{
    public class MyListingsUnitTest
    {
        GeekiumContext context = new GeekiumContext();

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

        #region Sell Listings
        // Call CreateSell()
        // Pass the initialized sell listing
        // Model returns okay
        [Fact]
        public void CreateSell_InputViableSellData_ModelReturnsValid()
        {
            // Arrange
            MyListingsController controller = new MyListingsController(context);
            SellListing list = InitializeSellListing();

            // Act
            controller.CreateSell(list);

            // Assert
            Assert.True(controller.ModelState.IsValid);
        }

        // Call CreateSell()
        // Pass the null data
        // An exception is returned
        [Fact]
        public void CreateSell__InsertNullData_NullException()
        {
            // Arrange
            MyListingsController controller = new MyListingsController(context);

            // Act
            var ex = controller.CreateSell(null);

            // Assert
            Assert.NotNull(ex.Exception);
        }

        // Call SellDetails()
        // Pass the initialized sell listing id
        // Result should be okay
        [Fact]
        public async Task SellDetails_PassInInitializedSellListingId_ReturnNoErrors()
        {
            // Arrange
            MyListingsController controller = new MyListingsController(context);
            SellListing list = InitializeSellListing();

            // Act
            context.Add(list);
            // list.SellListingid = 0, but adding to context changes it to 1
            var actionResult = await controller.SellDetails(1);

            // Assert
            Assert.IsType<ViewResult>(actionResult);
        }

        // Call SellDetails()
        // Pass in null
        // No result found error should be returned
        [Fact]
        public void SellDetails_PassInNull_ReturnNotFound()
        {
            // Arrange
            MyListingsController controller = new MyListingsController(context);
            
            // Act
            var actionResult = controller.SellDetails(null);

            // Assert
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        #region Construction
        //[Fact]
        // public async Task DeleteConfirmedSelling_PassInInitializedSellListingId_ReturnOk()
        // {
        //     // Assert
        //     MyListingsController controller = new MyListingsController(context);
        //     SellListing list = InitializeSellListing();
        //     InitializeSeller();

        //     // Act
        //     context.Add(list);
        //     var result = await controller.DeleteConfirmedSelling(0);

        //     // Assert
        //     Assert.IsType<OkResult>(result);
        // }


        // [Fact]
        // public async Task EditSell_PassSellIdAndModifiedSellListing_ModelIsValid()
        // {
        //     // Assert
        //     MyListingsController controller = new MyListingsController(context);
        //     SellListing list = InitializeSellListing();


        //     // Act
        //     context.Add(list);
        //     list.SellPrice = 555555;
        //     await controller.EditSell(list.SellListingId, list);

        //     Assert.True(controller.ModelState.IsValid);
        // }
        #endregion
        #endregion

        #region Trade Listings
        // Call CreateTrade()
        // Pass the initialized sell listing
        // Model returns okay
        [Fact]
        public void CreateTrade_InputViableTradeData_ModelReturnsValid()
        {
            // Arrange
            MyListingsController controller = new MyListingsController(context);
            TradeListing list = InitializeTradeListing();

            // Act
            controller.CreateTrade(list);

            // Assert
            Assert.True(controller.ModelState.IsValid);
        }

        // Call CreateTrade()
        // Pass the null data
        // An exception is returned
        [Fact]
        public void CreateTrade__InsertNullData_NullException()
        {
            // Arrange
            MyListingsController controller = new MyListingsController(context);

            // Act
            var ex = controller.CreateTrade(null);

            // Assert
            Assert.NotNull(ex.Exception);
        }


        // Call TradeDetails()
        // Pass in null
        // No result found error should be returned
        [Fact]
        public void TradeDetails_PassInNull_ReturnNotFound()
        {
            // Arrange
            MyListingsController controller = new MyListingsController(context);

            // Act
            var actionResult = controller.TradeDetails(null);

            // Assert
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        #endregion

        #region Service Listings
        // Call CreateService()
        // Pass the initialized sell listing
        // Model returns okay
        [Fact]
        public void CreateService_InputViableSellData_ModelReturnsValid()
        {
            // Arrange
            MyListingsController controller = new MyListingsController(context);
            ServiceListing list = InitializeServiceListing();

            // Act
            context.Add(InitializeAccount());
            controller.CreateService(list);

            // Assert
            Assert.True(controller.ModelState.IsValid);
        }

        // Call CreateService()
        // Pass the null data
        // An exception is returned
        [Fact]
        public void CreateService__InsertNullData_NullException()
        {
            // Arrange
            MyListingsController controller = new MyListingsController(context);

            // Act
            var ex = controller.CreateService(null);

            // Assert
            Assert.NotNull(ex.Exception);
        }


        // Call ServiceDetails()
        // Pass in null
        // No result found error should be returned
        [Fact]
        public void ServiceDetails_PassInNull_ReturnNotFound()
        {
            // Arrange
            MyListingsController controller = new MyListingsController(context);

            // Act
            var actionResult = controller.ServiceDetails(null);

            // Assert
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        #endregion
    }
}
