using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Geekium.Models
{
    public partial class Accounts
    {
        public Accounts()
        {
            AccountPurchases = new HashSet<AccountPurchases>();
            Cart = new HashSet<Cart>();
            Rewards = new HashSet<Rewards>();
            SellerAccounts = new HashSet<SellerAccounts>();
            ServiceListings = new HashSet<ServiceListings>();
        }

        public int AccountId { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? PointBalance { get; set; }

        public virtual ICollection<AccountPurchases> AccountPurchases { get; set; }
        public virtual ICollection<Cart> Cart { get; set; }
        public virtual ICollection<Rewards> Rewards { get; set; }
        public virtual ICollection<SellerAccounts> SellerAccounts { get; set; }
        public virtual ICollection<ServiceListings> ServiceListings { get; set; }
    }
}
