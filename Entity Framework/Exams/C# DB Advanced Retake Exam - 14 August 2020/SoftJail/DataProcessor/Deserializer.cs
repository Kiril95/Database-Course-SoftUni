namespace SoftJail.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var deserialize = JsonConvert.DeserializeObject<JsonDepartmentImportDTO[]>(jsonString);

            foreach (var departmentItem in deserialize)
            {
                if (!IsValid(departmentItem))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Department department = new Department()
                {
                    Name = departmentItem.Name,
                };

                foreach (var cellItem in departmentItem.Cells)
                {
                    if (!IsValid(cellItem))
                    {
                        sb.AppendLine("Invalid Data");
                        break;
                    }

                    department.Cells.Add(new Cell
                    {
                        CellNumber = cellItem.CellNumber,
                        HasWindow = cellItem.HasWindow
                    });
                }

                if (department.Cells.Any())
                {
                    context.Departments.Add(department);
                    context.SaveChanges();

                    sb.AppendLine($"Imported {department.Name} with {department.Cells.Count()} cells");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var deserialize = JsonConvert.DeserializeObject<JsonPrisonerImportDTO[]>(jsonString);

            foreach (var prisonerItem in deserialize)
            {
                if (!IsValid(prisonerItem))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                DateTime incarceration;
                bool checkIncarcerationDate = DateTime.TryParseExact(prisonerItem.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out incarceration);
                DateTime release;
                bool checkReleaseDate = DateTime.TryParseExact(prisonerItem.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out release);

                if (!checkIncarcerationDate || !checkReleaseDate)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Prisoner prisoner = new Prisoner()
                {
                    FullName = prisonerItem.FullName,
                    Nickname = prisonerItem.Nickname,
                    Age = prisonerItem.Age,
                    IncarcerationDate = incarceration,
                    ReleaseDate = release,
                    Bail = prisonerItem.Bail,
                    CellId = prisonerItem.CellId
                };

                bool isValidMail = true;

                foreach (var mailItem in prisonerItem.Mails)
                {
                    if (!IsValid(mailItem))
                    {
                        sb.AppendLine("Invalid Data");
                        isValidMail = false;
                        break;
                    }

                    prisoner.Mails.Add(new Mail
                    {
                        Description = mailItem.Description,
                        Sender = mailItem.Sender,
                        Address = mailItem.Address
                    });
                }

                if (isValidMail)
                {
                    context.Prisoners.Add(prisoner);
                    context.SaveChanges();

                    sb.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            List<Officer> officers = new List<Officer>();

            XmlSerializer serializer = new XmlSerializer(typeof(XmlOfficersImportDTO[]), new XmlRootAttribute("Officers"));
            using StringReader reader = new StringReader(xmlString);

            var deserialize = (XmlOfficersImportDTO[])serializer.Deserialize(reader);

            foreach (var officerItem in deserialize)
            {
                if (!IsValid(officerItem))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Position positionType;
                bool checkPositionType = Enum.TryParse(officerItem.Position, out positionType);
                Weapon weaponType;
                bool checkWeaponType = Enum.TryParse(officerItem.Weapon, out weaponType);

                if (!checkPositionType || !checkWeaponType)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Officer officer = new Officer()
                {
                    FullName = officerItem.FullName,
                    Salary = officerItem.Salary,
                    Position = positionType,
                    Weapon = weaponType,
                    DepartmentId = officerItem.DepartmentId,
                    OfficerPrisoners = officerItem.Prisoners.Select(x => new OfficerPrisoner
                    {
                        PrisonerId = x.Id
                    })
                    .ToArray()
                };

                context.Officers.Add(officer);
                context.SaveChanges();

                sb.AppendLine($"Imported {officer.FullName} ({officer.OfficerPrisoners.Count()} prisoners)");
            }

            Console.WriteLine(context.Officers.Count());
            Console.WriteLine(context.OfficersPrisoners.Count());

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}