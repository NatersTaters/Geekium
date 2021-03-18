using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Geekium.Models
{
    public partial class Account
    {
        public Account()
        {
            AccountPurchases = new HashSet<AccountPurchase>();
            Cart = new HashSet<Cart>();
            Rewards = new HashSet<Reward>();
            SellerAccounts = new HashSet<SellerAccount>();
            ServiceListings = new HashSet<ServiceListing>();
        }

        public int AccountId { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string PaswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? PointBalance { get; set; }

        public virtual ICollection<AccountPurchase> AccountPurchases { get; set; }
        public virtual ICollection<Cart> Cart { get; set; }
        public virtual ICollection<Reward> Rewards { get; set; }
        public virtual ICollection<SellerAccount> SellerAccounts { get; set; }
        public virtual ICollection<ServiceListing> ServiceListings { get; set; }
    }
}
