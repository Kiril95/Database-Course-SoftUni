using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Officer")]
    public class XmlOfficersImportDTO
    {
        [Required]
        [StringLength(30, MinimumLength = 3)]
        [XmlElement("Name")]
        public string FullName { get; set; }

        [Range(0.00, double.MaxValue)]
        [XmlElement("Money")]
        public decimal Salary { get; set; }

        [Required]
        [XmlElement("Position")]
        public string Position { get; set; }

        [Required]
        [XmlElement("Weapon")]
        public string Weapon { get; set; }

        [XmlElement("DepartmentId")]
        public int DepartmentId { get; set; }

        [XmlArray("Prisoners")]
        public XmlPrisonerImportDTO[] Prisoners { get; set; }
    }

    [XmlType("Prisoner")]
    public class XmlPrisonerImportDTO
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}
