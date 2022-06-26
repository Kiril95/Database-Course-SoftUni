using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ExportDto
{
    [XmlType("Project")]
    public class ProjectsExportDTO
    {
        [XmlAttribute("TasksCount")]
        public int TasksCount { get; set; }

        [XmlElement("ProjectName")]
        public string ProjectName { get; set; }

        [XmlElement("HasEndDate")]
        public string HasEndDate { get; set; }

        [XmlArray("Tasks")]
        public TaskExportDTO[] Tasks { get; set; }
    }

    [XmlType("Task")]
    public class TaskExportDTO
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Label")]
        public string LabelType { get; set; }
    }
}
