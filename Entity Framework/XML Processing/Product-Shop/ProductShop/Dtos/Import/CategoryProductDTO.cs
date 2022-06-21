using System.Xml.Serialization;

namespace ProductShop.Dtos.Import
{
    [XmlType("CategoryProduct")]
    public class CategoryProductDTO
    {
        [XmlElement]
        public int CategoryId { get; set; }


        [XmlElement]
        public int ProductId { get; set; }
    }
}
