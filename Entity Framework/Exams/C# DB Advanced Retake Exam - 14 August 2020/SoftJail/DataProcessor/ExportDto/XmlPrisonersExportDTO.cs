using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ExportDto
{
    [XmlType("Prisoner")]
    public class XmlPrisonerExportDTO
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("IncarcerationDate")]
        public string IncarcerationDate { get; set; }

        [XmlArray("EncryptedMessages")]
        public XmlMailExportDTO[] Messages { get; set; }
    }

    [XmlType("Message")]
    public class XmlMailExportDTO
    {
        [XmlElement("Description")]
        public string Description { get; set; }
    }
}
