﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Geekium.Models
{
    public partial class Receipt
    {
        public int ReceiptId { get; set; }
        public int CartId { get; set; }

        public virtual Cart Cart { get; set; }
    }
}
