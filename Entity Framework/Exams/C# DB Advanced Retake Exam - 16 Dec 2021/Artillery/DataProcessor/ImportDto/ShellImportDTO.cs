using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType("Shell")]
    public class ShellImportDTO
    {
        [Range(2.00, 1680.00)]
        [XmlElement("ShellWeight")]
        public double ShellWeight { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 4)]
        [XmlElement("Caliber")]
        public string Caliber { get; set; }
    }
}
