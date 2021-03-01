using System;
using System.Collections.Generic;

#nullable disable

namespace Geekium.Models
{
    public partial class SellerReview
    {
        public int SellerReviewId { get; set; }
        public int SellerId { get; set; }
        public int? BuyerRating { get; set; }
        public string ReviewDescription { get; set; }

        public virtual SellerAccount Seller { get; set; }
    }
}
