﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Geekium.Models
{
    public partial class PriceTrend
    {
        public PriceTrend()
        {
            SellListings = new HashSet<SellListing>();
        }

        public int PriceTrendId { get; set; }
        public string ItemName { get; set; }
        public DateTime DateOfUpdate { get; set; }
        public int? AveragePrice { get; set; }
        public int? LowestPrice { get; set; }
        public int? HighestPrice { get; set; }

        public virtual ICollection<SellListing> SellListings { get; set; }
    }
}
