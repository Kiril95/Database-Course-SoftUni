using System.ComponentModel.DataAnnotations;

namespace CodeFirstApproach.Models
{
    public class Country
    {
        public Country()
        {
            this.Leagues = new HashSet<League>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = null!;

        [MaxLength(30)]
        public string? TeamColor { get; set; }

        [MaxLength(50)]
        public string? NationalStadium { get; set; }

        public ICollection<League>? Leagues { get; set; }
    }
}
