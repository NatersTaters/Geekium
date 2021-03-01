using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Geekium.Models
{
    public partial class SellerAccounts
    {
        public SellerAccounts()
        {
            AccountPurchases = new HashSet<AccountPurchases>();
            SellListings = new HashSet<SellListings>();
            SellerReviews = new HashSet<SellerReviews>();
            TradeListings = new HashSet<TradeListings>();
        }

        public int SellerId { get; set; }
        public int AccountId { get; set; }
        public int? AverageRating { get; set; }

        public virtual Accounts Account { get; set; }
        public virtual ICollection<AccountPurchases> AccountPurchases { get; set; }
        public virtual ICollection<SellListings> SellListings { get; set; }
        public virtual ICollection<SellerReviews> SellerReviews { get; set; }
        public virtual ICollection<TradeListings> TradeListings { get; set; }
    }
}
