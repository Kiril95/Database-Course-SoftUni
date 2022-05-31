using System;
using System.Collections.Generic;

namespace DatabaseFirstApproach.Models
{
    public partial class GameType
    {
        public GameType()
        {
            Games = new HashSet<Game>();
            Items = new HashSet<Item>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? BonusStatsId { get; set; }

        public virtual Statistic? BonusStats { get; set; }
        public virtual ICollection<Game> Games { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}
