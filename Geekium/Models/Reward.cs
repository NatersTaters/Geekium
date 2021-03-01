using System;
using System.Collections.Generic;

#nullable disable

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
