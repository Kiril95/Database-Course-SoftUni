using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace P03_FootballBetting.Data.Models
{
    public class Position
    {
        public Position()
        {
            this.Players = new HashSet<Player>();
        }

        [Key]
        public int PositionId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string Name { get; set; }

        public virtual ICollection<Player> Players { get; set; }
    }
}
