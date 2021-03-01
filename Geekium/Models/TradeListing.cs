using System;
using System.Collections.Generic;

#nullable disable

namespace Geekium.Models
{
    public partial class TradeListing
    {
        public int TradeListingId { get; set; }
        public int SellerId { get; set; }
        public string TradeTitle { get; set; }
        public string TradeDescription { get; set; }
        public string TradeFor { get; set; }
        public DateTime TradeDate { get; set; }
        public string TradeItemType { get; set; }
        public int TradeQuantity { get; set; }

        public virtual SellerAccount Seller { get; set; }
    }
}
