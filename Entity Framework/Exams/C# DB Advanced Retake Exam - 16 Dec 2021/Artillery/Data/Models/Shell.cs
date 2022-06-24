using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Artillery.Data.Models
{
    public class Shell
    {
        public Shell()
        {
            this.Guns = new HashSet<Gun>();
        }

        public int Id { get; set; }

        [Range(2.00, 1680.00)]
        public double ShellWeight { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 4)]
        public string Caliber { get; set; }

        public virtual ICollection<Gun> Guns { get; set; }
    }
}
