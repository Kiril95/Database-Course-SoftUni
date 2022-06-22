using System.Xml.Serialization;

namespace CarDealer.Dtos.Import
{
    [XmlType("Part")]
    public class PartDTO
    {

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("Price")]
        public decimal price { get; set; }

        [XmlElement("quantity")]
        public int Quantity { get; set; }

        [XmlElement("supplierId")]
        public int SupplierId { get; set; }
    }
}
