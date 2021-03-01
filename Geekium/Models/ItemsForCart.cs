using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Geekium.Models
{
    public partial class ItemsForCart
    {
        public int ItemsForCartId { get; set; }
        public int CartId { get; set; }
        public int SellListingId { get; set; }
        public int Quantity { get; set; }

        public virtual Cart Cart { get; set; }
        public virtual SellListings SellListing { get; set; }
    }
}
