using System.ComponentModel.DataAnnotations;

namespace CodeFirstApproach.Models
{
    public class League
    {
        public League()
        {
            this.Teams = new HashSet<Team>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; } = null!;

        public int CountryId { get; set; }

        public virtual Country? Country { get; set; }

        public ICollection<Team>? Teams { get; set; }
    }
}
