using Geekium.Controllers;
using Geekium.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTestGeekium
{
    public class SellerReviewsUnitTest
    {
        GeekiumContext context = new GeekiumContext();

        void InitializeSeller()
        {
            SellerAccount seller = new SellerAccount
            {
                AccountId = 0,
                AverageRating = 0,
                Account = InitializeAccount()
            };

            context.Add(seller);
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
            context.Add(account);
            return account;
        }


        SellerReview InitializeSellerReview()
        {
            SellerReview review = new SellerReview
            {
                SellerId = 0,
                AccountId = 0,
                BuyerRating = 3,
                ReviewDescription = "Good item",
                Account = InitializeAccount()
            };
            context.Add(review);
            return review;
        }


        // Call Index()
        // Pass in an id of 0
        // View should be returned
        [Fact]
        public async void Index_CallFunction_ReturnView()
        {
            // Arrange
            SellerReviewsController controller = new SellerReviewsController(context);

            // Act
            var actionResult = await controller.Index(0);

            // Assert
            Assert.IsType<ViewResult>(actionResult);
        }

        // Call Details()
        // Pass in null
        // Return not found
        [Fact]
        public async void Details_PassInNull_ReturnNotFound()
        {
            // Arrange
            SellerReviewsController controller = new SellerReviewsController(context);

            // Act
            var actionResult = await controller.Details(null);

            // Assert
            Assert.IsType<NotFoundResult>(actionResult);
        }

        // Call Create()
        // Send in a seller review
        // Return valid model
        [Fact]
        public async void Create_PassInSellerReview_ReturnValidModel()
        {
            // Arrange
            SellerReviewsController controller = new SellerReviewsController(context);

            InitializeSeller();
            var sellerReview = InitializeSellerReview();

            // Act
            await controller.Create(sellerReview);

            // Assert
            Assert.True(controller.ModelState.IsValid);
        }

        // Call Edit
        // Send in a review id 0 and seller review object
        // Return valid model
        [Fact]
        public async void Edit_PassIn0AndSellerReview_ReturnNotFound()
        {
            // Arrange
            SellerReviewsController controller = new SellerReviewsController(context);

            InitializeSeller();
            var sellerReview = InitializeSellerReview();

            // Act
            await controller.Edit(0, sellerReview);

            // Assert
            Assert.True(controller.ModelState.IsValid);
        }

        // Call DeleteConfirmed
        // Send in a review id 0
        // Return view
        [Fact]
        public async void DeleteConfirmed_PassIn0_ReturnNotFound()
        {
            // Arrange
            SellerReviewsController controller = new SellerReviewsController(context);

            InitializeSeller();
            var sellerReview = InitializeSellerReview();

            // Act
            var actionResult = await controller.DeleteConfirmed(0);

            // Assert
            Assert.IsType<RedirectToActionResult>(actionResult);
        }

    }
}
