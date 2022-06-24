using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Artillery.Data.Models
{
    public class Country
    {
        public Country()
        {
            this.CountriesGuns = new HashSet<CountryGun>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 4)]
        public string CountryName { get; set; }

        [Range(50000, 1000000)]
        public int ArmySize { get; set; }

        public virtual ICollection<CountryGun> CountriesGuns { get; set; }
    }
}
