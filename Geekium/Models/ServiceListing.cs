using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Geekium.Models
{
    public partial class ServiceListing
    {
        public int ServiceListingId { get; set; }
        public int AccountId { get; set; }
        public string ServiceTitle { get; set; }
        public string ServiceDescription { get; set; }
        public DateTime ListingDate { get; set; }
        public byte[] ServiceImage { get; set; }

        public virtual Account Account { get; set; }
    }
}
