namespace SoftJail.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using SoftJail.DataProcessor.ExportDto;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context.Prisoners
                .ToArray()
                .Where(p => ids.Contains(p.Id))
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.FullName,
                    CellNumber = x.Cell.CellNumber,
                    Officers = x.PrisonerOfficers.Select(po => new
                    {
                        OfficerName = po.Officer.FullName,
                        Department = po.Officer.Department.Name
                    })
                    .OrderBy(x => x.OfficerName)
                    .ToArray(),
                    TotalOfficerSalary = decimal.Parse(x.PrisonerOfficers.Sum(po => po.Officer.Salary).ToString("f2"))
                })
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Id)
                .ToArray();

            return JsonConvert.SerializeObject(prisoners, Formatting.Indented);
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(XmlPrisonerExportDTO[]), new XmlRootAttribute("Prisoners"));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            using StringWriter writer = new StringWriter(sb);

            string[] targetPrisoners = prisonersNames.Split(",", StringSplitOptions.RemoveEmptyEntries).ToArray();

            var prisoners = context.Prisoners
                .ToArray()
                .Where(x => targetPrisoners.Contains(x.FullName))
                .Select(p => new XmlPrisonerExportDTO
                {
                    Id = p.Id,
                    Name = p.FullName,
                    IncarcerationDate = p.IncarcerationDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Messages = p.Mails.Select(m => new XmlMailExportDTO
                    {
                        Description = new string(m.Description.ToCharArray().Reverse().ToArray())
                    })
                    .ToArray()
                })
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Id)
                .ToArray();

            serializer.Serialize(writer, prisoners, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}