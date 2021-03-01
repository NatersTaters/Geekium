using System;
using System.Collections.Generic;

#nullable disable

namespace Geekium.Models
{
    public partial class Account
    {
        public Account()
        {
            AccountPurchases = new HashSet<AccountPurchase>();
            Carts = new HashSet<Cart>();
            Rewards = new HashSet<Reward>();
            SellerAccounts = new HashSet<SellerAccount>();
            ServiceListings = new HashSet<ServiceListing>();
        }

        public int AccountId { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? PointBalance { get; set; }

        public virtual ICollection<AccountPurchase> AccountPurchases { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Reward> Rewards { get; set; }
        public virtual ICollection<SellerAccount> SellerAccounts { get; set; }
        public virtual ICollection<ServiceListing> ServiceListings { get; set; }
    }
}
