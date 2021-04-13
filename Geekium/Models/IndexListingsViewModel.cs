using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Geekium.Models
{
	public class IndexListingsViewModel
	{
		public IEnumerable<SellListing> SellListings { get; set; }
		public IEnumerable<TradeListing> TradeListings { get; set; }
		public IEnumerable<ServiceListing> ServiceListings { get; set; }
	}
}
