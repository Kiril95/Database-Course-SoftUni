namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;

    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.Data.Models;
    using TeisterMask.Data.Models.Enums;
    using TeisterMask.DataProcessor.ImportDto;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            List<Project> projects = new List<Project>();

            XmlSerializer serializer = new XmlSerializer(typeof(ProjectsImportDTO[]), new XmlRootAttribute("Projects"));
            using StringReader reader = new StringReader(xmlString);

            var deserialize = (ProjectsImportDTO[])serializer.Deserialize(reader);

            foreach (var projectItem in deserialize)
            {
                if (!IsValid(projectItem))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime projectOpenDate;
                DateTime projectDueDate;
                var checkProjectOpenDate = DateTime.TryParseExact(projectItem.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out projectOpenDate);
                var checkProjectDueDate = DateTime.TryParseExact(projectItem.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out projectDueDate);

                Project project = new Project()
                {
                    Name = projectItem.Name,
                    OpenDate = projectOpenDate,
                    DueDate = projectDueDate,
                };

                foreach (var taskItem in projectItem.Tasks)
                {
                    if (!IsValid(taskItem))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime taskOpenDate;
                    DateTime taskDueDate;
                    var checkTaskOpenDate = DateTime.TryParseExact(taskItem.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out taskOpenDate);
                    var checkTaskDueDate = DateTime.TryParseExact(taskItem.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out taskDueDate);

                    if (!checkTaskOpenDate || !checkTaskDueDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (taskOpenDate < projectOpenDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (projectDueDate.Year > 0001 && taskDueDate > projectDueDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    project.Tasks.Add(new Task()
                    {
                        Name = taskItem.Name,
                        OpenDate = taskOpenDate,
                        DueDate = taskDueDate,
                        ExecutionType = (ExecutionType)taskItem.ExecutionType,
                        LabelType = (LabelType)taskItem.LabelType
                    });
                }

                projects.Add(project);
                sb.AppendLine(string.Format(SuccessfullyImportedProject, project.Name, project.Tasks.Count()));
            }

            context.Projects.AddRange(projects);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            List<Employee> employees = new List<Employee>();

            var deserialize = JsonConvert.DeserializeObject<IEnumerable<EmployeesImportDTO>>(jsonString);

            foreach (var employeeItem in deserialize)
            {
                if (!IsValid(employeeItem))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Employee employee = new Employee()
                {
                    Username = employeeItem.Username,
                    Email = employeeItem.Email,
                    Phone = employeeItem.Phone
                };

                foreach (var currentTaskId in employeeItem.Tasks.Distinct())
                {
                    var taskIds = employee.EmployeesTasks.Select(x => x.TaskId);
                    var targetTask = context.Tasks.FirstOrDefault(x => x.Id == currentTaskId);

                    if (targetTask is null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (!taskIds.Contains(currentTaskId))
                    {
                        employee.EmployeesTasks.Add(new EmployeeTask
                        {
                            EmployeeId = employee.Id,
                            TaskId = currentTaskId
                        });
                    }
                }

                employees.Add(employee);
                sb.AppendLine(string.Format(SuccessfullyImportedEmployee, employee.Username, employee.EmployeesTasks.Count()));
            }

            context.Employees.AddRange(employees);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}