using System;
using System.Collections.Generic;

#nullable disable

namespace Geekium.Models
{
    public partial class AccountPurchase
    {
        public int AccountPurchaseId { get; set; }
        public int AccountId { get; set; }
        public int SellerId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int PurchasePrice { get; set; }
        public int TrackingNumber { get; set; }
        public string SellerName { get; set; }
        public int PointsGained { get; set; }

        public virtual Account Account { get; set; }
        public virtual SellerAccount Seller { get; set; }
    }
}
