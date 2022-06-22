using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType("User")]
    public class UsersWithProductsDTO
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlElement("age")]
        public int? Age { get; set; }

        [XmlElement("SoldProducts")]
        public UserSoldProduct SoldProducts { get; set; }
    }

    [XmlType("SoldProducts")]
    public class UserSoldProduct
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("products")]
        public SoldProductDTO[] Products { get; set; }
    }

    [XmlType("Users")]
    public class ExportModel
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("users")]
        public UsersWithProductsDTO[] Users { get; set; }
    }
}
