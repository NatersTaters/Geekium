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
			return View();
		}

		public IActionResult SendSupport()
		{
			SendEmail();
			return View("SupportSubmit");
		}

		public void SendEmail()
		{
			var senderEmail = new MailAddress("geekium1234@gmail.com");
			var receiverEmail = new MailAddress(HttpContext.Session.GetString("userEmail"));
			var password = "geekiumaccount1234";
			var sub = "Account Support";
			var body = "Your support ticket has been submitted, please wait 3 to 5 buisness days for a response";
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

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
