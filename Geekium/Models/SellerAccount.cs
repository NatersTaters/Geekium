using System;
using System.Collections.Generic;

#nullable disable

namespace Geekium.Models
{
    public partial class SellerAccount
    {
        public SellerAccount()
        {
            AccountPurchases = new HashSet<AccountPurchase>();
            SellListings = new HashSet<SellListing>();
            SellerReviews = new HashSet<SellerReview>();
            TradeListings = new HashSet<TradeListing>();
        }

        public int SellerId { get; set; }
        public int AccountId { get; set; }
        public int? AverageRating { get; set; }

        public virtual Account Account { get; set; }
        public virtual ICollection<AccountPurchase> AccountPurchases { get; set; }
        public virtual ICollection<SellListing> SellListings { get; set; }
        public virtual ICollection<SellerReview> SellerReviews { get; set; }
        public virtual ICollection<TradeListing> TradeListings { get; set; }
    }
}
