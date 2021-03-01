using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Geekium.Models
{
    public partial class AccountPurchases
    {
        public int AccountPurchaseId { get; set; }
        public int AccountId { get; set; }
        public int SellerId { get; set; }
        public int CartId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int PurchasePrice { get; set; }
        public int TrackingNumber { get; set; }
        public string SellerName { get; set; }
        public int PointsGained { get; set; }

        public virtual Accounts Account { get; set; }
        public virtual Cart Cart { get; set; }
        public virtual SellerAccounts Seller { get; set; }
    }
}
