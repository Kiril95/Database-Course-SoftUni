using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstApproach.Models
{
    [Index(nameof(Name), IsUnique = true)]
    [Index(nameof(ClubStadium), IsUnique = true)]
    public class Team
    {
        public Team()
        {
            this.Players = new HashSet<Player>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; } = null!;

        public int LeagueId { get; set; }

        [Required]
        [MaxLength(30)]
        public string TeamColor { get; set; } = null!;

        [Required]
        [MaxLength(30)]
        public string ClubStadium { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Owner { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Coach { get; set; } = null!;

        public int Titles { get; set; }

        public virtual League? League { get; set; }

        public ICollection<Player>? Players { get; set; }
    }
}
