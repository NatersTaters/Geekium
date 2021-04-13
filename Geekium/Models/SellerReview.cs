using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Geekium.Models
{
    public partial class SellerReview
    {
        public int SellerReviewId { get; set; }
        public int SellerId { get; set; }
        public int AccountId { get; set; }
        public double? BuyerRating { get; set; }
        public string ReviewDescription { get; set; }

        public virtual Account Account { get; set; }
        public virtual SellerAccount Seller { get; set; }
    }
}
