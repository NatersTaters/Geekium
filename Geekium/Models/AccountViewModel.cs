using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Geekium.Models
{
	public class AccountViewModel
	{
		[Required]
		[StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long", MinimumLength = 6)]
		public string Username { get; set; }

		[Required]
		[StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[Required]
		[StringLength(50, ErrorMessage = "The First Name must be less than {1}")] 
		public string FirstName { get; set; }

		[Required]
		[StringLength(50, ErrorMessage = "The First Name must be less than {1}")]
		public string LastName { get; set; }
		
		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }

		public string ReturnUrl { get; set; }
	}
}
