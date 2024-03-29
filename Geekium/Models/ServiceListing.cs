﻿using Microsoft.AspNetCore.Http;
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
    public partial class ServiceListing
    {
        public int ServiceListingId { get; set; }
        public int AccountId { get; set; }
        public string ServiceTitle { get; set; }
        public string ServiceDescription { get; set; }
        [DataType(DataType.Date)]
        public DateTime ListingDate { get; set; }
        public string ServiceLocation { get; set; }
        public string ServiceImage { get; set; }

        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }

        public virtual Account Account { get; set; }
    }
}
