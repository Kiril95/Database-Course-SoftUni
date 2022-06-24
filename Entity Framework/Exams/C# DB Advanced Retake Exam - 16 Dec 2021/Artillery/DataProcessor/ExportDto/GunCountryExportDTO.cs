using System.Xml.Serialization;

namespace Artillery.DataProcessor.ExportDto
{
    [XmlType("Gun")]
    public class GunCountryExportDTO
    {
        [XmlAttribute("Manufacturer")]
        public string Manufacturer { get; set; }

        [XmlAttribute("GunType")]
        public string GunType { get; set; }

        [XmlAttribute("GunWeight")]
        public int GunWeight { get; set; }

        [XmlAttribute("BarrelLength")]
        public double Barrellength { get; set; }

        [XmlAttribute("Range")]
        public int Range { get; set; }

        [XmlArray("Countries")]
        public CountryExportDTO[] Countries { get; set; }
    }

    [XmlType("Country")]
    public class CountryExportDTO
    {
        [XmlAttribute("Country")]
        public string Country { get; set; }

        [XmlAttribute("ArmySize")]
        public int ArmySize { get; set; }
    }
}
