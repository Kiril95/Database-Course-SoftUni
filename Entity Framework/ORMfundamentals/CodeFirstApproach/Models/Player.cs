using System.ComponentModel.DataAnnotations;

namespace CodeFirstApproach.Models
{
    public class Player
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        public int Age { get; set; }

        [Required]
        public string Position { get; set; } = null!;

        public int TeamId { get; set; }

        public virtual Team? Team { get; set; }


    }
}
