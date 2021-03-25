using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Geekium.Models
{
	public class UpgradeViewModel
	{
		[Required]
		[StringLength(6, ErrorMessage = "The code cannot be greater than {1}")]
		public string Code { get; set; }

		public string ReturnUrl { get; set; }
	}
}
