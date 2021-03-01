using System;
using System.Collections.Generic;

#nullable disable

namespace Geekium.Models
{
    public partial class ServiceListing
    {
        public int ServiceListingId { get; set; }
        public int AccountId { get; set; }
        public string ServiceTitle { get; set; }
        public string ServiceDescription { get; set; }
        public DateTime ListingDate { get; set; }

        public virtual Account Account { get; set; }
    }
}
