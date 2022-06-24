using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType("Country")]
    public class CountryImportDTO
    {
        [Required]
        [StringLength(60, MinimumLength = 4)]
        [XmlElement("CountryName")]
        public string CountryName { get; set; }

        [XmlElement("ArmySize")]
        [Range(50000, 10000000)]
        public int ArmySize { get; set; }
    }
}
