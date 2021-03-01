using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Geekium.Models
{
    public partial class Cart
    {
        public Cart()
        {
            AccountPurchases = new HashSet<AccountPurchase>();
            ItemsForCart = new HashSet<ItemsForCart>();
            Receipt = new HashSet<Receipt>();
        }

        public int CartId { get; set; }
        public int AccountId { get; set; }
        public bool? TransactionComplete { get; set; }
        public int? NumberOfProducts { get; set; }
        public int? TotalPrice { get; set; }
        public int? PointsGained { get; set; }

        public virtual Account Account { get; set; }
        public virtual ICollection<AccountPurchase> AccountPurchases { get; set; }
        public virtual ICollection<ItemsForCart> ItemsForCart { get; set; }
        public virtual ICollection<Receipt> Receipt { get; set; }
    }
}
