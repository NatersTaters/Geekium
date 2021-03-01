using System;
using System.Collections.Generic;

#nullable disable

namespace Geekium.Models
{
    public partial class SellListing
    {
        public SellListing()
        {
            ItemsForCarts = new HashSet<ItemsForCart>();
        }

        public int SellListingId { get; set; }
        public int SellerId { get; set; }
        public int PriceTrendId { get; set; }
        public string SellTitle { get; set; }
        public string SellDescription { get; set; }
        public int SellPrice { get; set; }
        public DateTime SellDate { get; set; }
        public string SellItemType { get; set; }
        public int SellQuantity { get; set; }

        public virtual PriceTrend PriceTrend { get; set; }
        public virtual SellerAccount Seller { get; set; }
        public virtual ICollection<ItemsForCart> ItemsForCarts { get; set; }
    }
}
