using System.Xml.Serialization;

namespace CarDealer.Dtos.Import
{
    [XmlType("partId")]
    public class CarPartDTO
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}
