using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Geekium.Models
{
    public partial class TradeListing
    {
        public int TradeListingId { get; set; }
        public int SellerId { get; set; }
        public string TradeTitle { get; set; }
        public string TradeDescription { get; set; }
        public string TradeFor { get; set; }
        [DataType(DataType.Date)]
        public DateTime TradeDate { get; set; }
        public string TradeItemType { get; set; }
        public int TradeQuantity { get; set; }
        public string TradeImage { get; set; }
        public string TradeLocation { get; set; }
        public bool? Display { get; set; }

        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }

        public virtual SellerAccount Seller { get; set; }
    }
}
