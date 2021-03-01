using System;
using System.Collections.Generic;

#nullable disable

namespace Geekium.Models
{
    public partial class ItemsForCart
    {
        public int ItemsForCartId { get; set; }
        public int CartId { get; set; }
        public int SellListingId { get; set; }

        public virtual Cart Cart { get; set; }
        public virtual SellListing SellListing { get; set; }
    }
}
