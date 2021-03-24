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
		public string Code { get; set; }

		public string ReturnUrl { get; set; }
	}
}
