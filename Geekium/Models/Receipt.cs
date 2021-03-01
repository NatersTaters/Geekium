using System;
using System.Collections.Generic;

#nullable disable

namespace Geekium.Models
{
    public partial class Receipt
    {
        public int ReceiptId { get; set; }
        public int CartId { get; set; }

        public virtual Cart Cart { get; set; }
    }
}
