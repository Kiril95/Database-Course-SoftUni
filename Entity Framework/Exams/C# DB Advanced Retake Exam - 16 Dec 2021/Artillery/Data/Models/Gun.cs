using Artillery.Data.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Artillery.Data.Models
{
    public class Gun
    {
        public Gun()
        {
            this.CountriesGuns = new HashSet<CountryGun>();
        }

        public int Id { get; set; }

        public int ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }

        [Range(100, 1350000)]
        public int GunWeight { get; set; }

        [Range(2.00, 35.00)]
        public double BarrelLength { get; set; }

        public int? NumberBuild { get; set; }

        [Range(1, 100000)]
        public int Range { get; set; }

        [Required]
        public GunType GunType { get; set; }

        public int ShellId { get; set; }
        public Shell Shell { get; set; }

        public virtual ICollection<CountryGun> CountriesGuns { get; set; }
    }
}