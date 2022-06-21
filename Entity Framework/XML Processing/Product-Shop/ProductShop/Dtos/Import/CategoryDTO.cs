using System.Xml.Serialization;

namespace ProductShop.Dtos.Import
{
    [XmlType("Category")]
    public class CategoryDTO
    {
        [XmlElement("name")]
        public string Name { get; set; }
    }
}
