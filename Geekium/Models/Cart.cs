using System;
using System.Collections.Generic;

#nullable disable

namespace Geekium.Models
{
    public partial class Cart
    {
        public Cart()
        {
            ItemsForCarts = new HashSet<ItemsForCart>();
            Receipts = new HashSet<Receipt>();
        }

        public int CartId { get; set; }
        public int AccountId { get; set; }
        public int? NumberOfProducts { get; set; }
        public int? TotalPrice { get; set; }
        public int? PointsGained { get; set; }

        public virtual Account Account { get; set; }
        public virtual ICollection<ItemsForCart> ItemsForCarts { get; set; }
        public virtual ICollection<Receipt> Receipts { get; set; }
    }
}
