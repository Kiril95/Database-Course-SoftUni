namespace TeisterMask.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.DataProcessor.ExportDto;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var employees = context.Employees
                .ToArray()
                .Where(x => x.EmployeesTasks.Any(et => et.Task.OpenDate >= date))
                .Select(x => new EmployeesExportDTO
                {
                    Username = x.Username,
                    Tasks = x.EmployeesTasks
                        .Where(et => et.Task.OpenDate >= date)
                        .OrderByDescending(x => x.Task.DueDate)
                        .ThenBy(x => x.Task.Name)
                        .Select(et => new TasksExportDTO
                        {
                            TaskName = et.Task.Name,
                            OpenDate = et.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                            DueDate = et.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                            LabelType = et.Task.LabelType.ToString(),
                            ExecutionType = et.Task.ExecutionType.ToString()
                        })
                        .ToArray(),
                })
                .OrderByDescending(x => x.Tasks.Count())
                .ThenBy(x => x.Username)
                .Take(10)
                .ToArray();

            return JsonConvert.SerializeObject(employees, Formatting.Indented);
        }

        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(ProjectsExportDTO[]), new XmlRootAttribute("Projects"));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            using StringWriter writer = new StringWriter(sb);

            var projects = context.Projects
                .ToArray()
                .Where(x => x.Tasks.Count() > 0)
                .Select(x => new ProjectsExportDTO
                {
                    TasksCount = x.Tasks.Count(),
                    ProjectName = x.Name,
                    HasEndDate = x.DueDate.HasValue ? "Yes" : "No",
                    Tasks = x.Tasks.Select(t => new TaskExportDTO
                    {
                        Name = t.Name,
                        LabelType = t.LabelType.ToString()
                    })
                    .OrderBy(x => x.Name)
                    .ToArray()
                })
                .OrderByDescending(x => x.TasksCount)
                .ThenBy(x => x.ProjectName)
                .ToArray();

            serializer.Serialize(writer, projects, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}