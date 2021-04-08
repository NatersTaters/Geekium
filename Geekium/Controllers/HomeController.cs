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

namespace Geekium.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			return View();
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
			//Send support confirmation email to the user account email address
			string accountSupportText = "Your support request has been submitted, please allow 3 to 5 buisness days for a responce";
			string userEmail = HttpContext.Session.GetString("userEmail");
			SendEmail(accountSupportText, userEmail);

			//Send support details to the admin email address
			string adminSupportText = support.SupportBody;
			string adminEmail = "geekium1234@gmail.com";
			SendEmail(adminSupportText, adminEmail);

			return View("Index");
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
	}
}
