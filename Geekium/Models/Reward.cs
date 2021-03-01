using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Geekium.Models
{
    public partial class Reward
    {
        public int RewardId { get; set; }
        public int AccountId { get; set; }
        public string RewardType { get; set; }
        public string RewardCode { get; set; }
        public int? PointCost { get; set; }
        public DateTime DateReceived { get; set; }

        public virtual Account Account { get; set; }
    }
}
