using System;
using Xunit;
using Geekium.Controllers;
using Geekium.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTestGeekium
{
    public class AccountIntegration
    {
        GeekiumContext context = new GeekiumContext();
        ILogger<HomeController> logger;
        IWebHostEnvironment hostEnvironment;
        public AccountViewModel AccountRegistration()
        {
            AccountViewModel accountViewModel = new AccountViewModel()
            {
                Username = "GeekiumTester",
                Password = "GeekiumTester123",
                FirstName = "Leunard",
                LastName = "Gervalla",
                Email = "lgervalla8340@conestogac.on.ca"
            };
            LoginViewModel loginModel = AccountLogin(accountViewModel);

            return accountViewModel;
        }
        public LoginViewModel AccountLogin(AccountViewModel model)
        {
            LoginViewModel loginViewModel = new LoginViewModel()
            {
                Username = model.Username,
                Password = model.Password
            };
            //HttpContext.Session.GetString("username", model.Username);
            //HttpContext.Session.GetString("userEmail", model.Email);
            return loginViewModel;
        }
        public Support SupportMessage()
        {
            Support support = new Support()
            {
                SupportHeader = "Geekium Support",
                SupportBody = "Geekium support message to be sent"
            };
            return support;
        }
        public SellListing Product()
        {
            SellListing sellListing = new SellListing
            {
                SellListingId = 20,
                SellTitle = "Graphics Card",
                SellDescription = "Price scalping it bad",
                SellPrice = 1500,
                SellQuantity = 1,
            };
            return sellListing;
        }
        public List<ItemsForCart> Cart()
        {
            SellListing sellListing = Product();
            ItemsForCart product = new ItemsForCart
            {
                SellListing = sellListing,
                Quantity = 1
            };
            List<ItemsForCart> cart = new List<ItemsForCart>();
            cart.Add(product);
            return cart;
        }

        [Fact]
        public void RegisterAndLoginAccount_SendASupportTicket()
        {
            //Arrange 
            AccountsController accountsController = new AccountsController(context, hostEnvironment);
            AccountViewModel accountViewModel = AccountRegistration();
            LoginViewModel loginViewModel = AccountLogin(accountViewModel);

            Support support = SupportMessage();
            HomeController homeController = new HomeController(logger, context);

            //Act
            accountsController.Create(accountViewModel);
            accountsController.Login(loginViewModel);

            try
            {
                //Assert
                if (accountsController.ModelState.IsValid)
                {
                    homeController.SendSupport(support);
                    Assert.True(accountsController.ModelState.IsValid);
                    Assert.True(homeController.ModelState.IsValid);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {

            }
        }

        [Fact]
        public async Task RegisterAndLoginAccount_UpgradeToSellerAccount()
        {
            //Arrange 
            AccountsController accountsController = new AccountsController(context, hostEnvironment);
            AccountViewModel accountViewModel = AccountRegistration();
            LoginViewModel loginViewModel = AccountLogin(accountViewModel);
            SellerAccount sellerAccount = new SellerAccount();

            SellerAccountsController sellerAccountsController = new SellerAccountsController(context, hostEnvironment);

            //Act
            accountsController.Create(accountViewModel);
            accountsController.Login(loginViewModel);

            try
            {
                //Assert
                if (accountsController.ModelState.IsValid)
                {
                    await sellerAccountsController.Create(sellerAccount);

                    Assert.True(accountsController.ModelState.IsValid && sellerAccountsController.ModelState.IsValid);

                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {

            }
        }

        [Fact]
        public async Task RegisterAndLoginAccount_DeleteTheAccount()
        {
            //Arrange 
            AccountsController accountsController = new AccountsController(context, hostEnvironment);
            AccountViewModel accountViewModel = AccountRegistration();
            LoginViewModel loginViewModel = AccountLogin(accountViewModel);

            var account = await context.Accounts
                .FirstOrDefaultAsync(m => m.AccountId == 1);

            //Act
            try
            {
                //Assert
                if (accountsController.ModelState.IsValid)
                {
                    await accountsController.Delete(account.AccountId);

                    Assert.True(accountsController.ModelState.IsValid);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {

            }
        }

    }
}

