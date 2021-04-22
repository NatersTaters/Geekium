using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Geekium.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;

namespace Geekium.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly GeekiumContext _context;

		public HomeController(ILogger<HomeController> logger, GeekiumContext context)
		{
			_logger = logger;
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			IndexListingsViewModel viewModel = new IndexListingsViewModel();

			viewModel.SellListings = GetSellListings();
			viewModel.TradeListings = GetTradeListings();
			viewModel.ServiceListings = GetServiceListings();

			return View(viewModel);
		}

		public IActionResult About()
		{
			return View();
		}

		public IActionResult FAQ()
		{
			return View();
		}

		public IActionResult Support()
		{
			if(HttpContext.Session.GetString("userEmail") == null)
			{
				return RedirectToAction("Login", "Accounts", new { area = "" });
			}
			else
			{
				return View();
			}
		}

		public IActionResult SendSupport(Support support)
		{
			if(ModelState.IsValid)
			{
				//Send support confirmation email to the user account email address
				string accountSupportText = "Your support request has been submitted, please allow 3 to 5 buisness days for a responce";
				string userEmail = HttpContext.Session.GetString("userEmail");
				SendEmail(accountSupportText, userEmail);

				//Send support details to the admin email address
				string adminSupportText = support.SupportBody;
				string adminEmail = "geekium1234@gmail.com";
				SendEmail(adminSupportText, adminEmail);

				return RedirectToAction("Index");
			}
			ModelState.AddModelError("", "Invalid Attempt");
			return View(support);
		}

		public void SendEmail(string messageBody, string emailAddress)
		{
			var senderEmail = new MailAddress("geekium1234@gmail.com");
			var receiverEmail = new MailAddress(emailAddress);
			var password = "geekiumaccount1234";
			var sub = "Account Support";
			var body = messageBody;
			var smtp = new SmtpClient
			{
				Host = "smtp.gmail.com",
				Port = 587,
				EnableSsl = true,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(senderEmail.Address, password)
			};
			using (var mess = new MailMessage(senderEmail, receiverEmail)
			{
				Subject = sub,
				Body = body
			})
			{
				smtp.Send(mess);
			}
		}

		public IActionResult Credit()
        {
			return View();
        }

		public IActionResult Error(int? statusCode = null)
		{
			if (statusCode.HasValue)
			{
				if (statusCode == 404 || statusCode == 500)
				{
					var viewName = statusCode.ToString();
					return View(viewName);
				}
			}

			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		public List<SellListing> GetSellListings()
		{
			List<SellListing> sellListings = new List<SellListing>();

			var sellListingContext = _context.SellListings.Include(s => s.PriceTrend)
				.Include(s => s.Seller)
				.Include(s => s.Seller.Account)
				.Where(s => s.Display == true);

			foreach(var item in sellListingContext)
			{
				sellListings.Add(item);
			}

			return sellListings;
		}

		public List<TradeListing> GetTradeListings()
		{
			List<TradeListing> tradeListings = new List<TradeListing>();

			var tradeListingContext = _context.TradeListings.Include(t => t.Seller)
				.Include(t => t.Seller.Account);

			foreach (var item in tradeListingContext)
			{
				tradeListings.Add(item);
			}

			return tradeListings;
		}

		public List<ServiceListing> GetServiceListings()
		{
			List<ServiceListing> serviceListings = new List<ServiceListing>();

			var serviceListingContext = _context.ServiceListings.Include(s => s.Account);

			foreach (var item in serviceListingContext)
			{
				serviceListings.Add(item);
			}

			return serviceListings;
		}
	}
}
