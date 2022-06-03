using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var db = new SoftUniContext();
            var exec = GetEmployeesFromResearchAndDevelopment(db);

            Console.WriteLine(exec);
        }

        public static string GetEmployeesFullInformation(SoftUniContext context) // Task - 0.3
        {
            var result = string.Empty;
            var employees = context.Employees.ToArray();

            foreach (var person in employees)
            {
                result += $"{person.FirstName} {person.LastName} {person.MiddleName} {person.JobTitle} {person.Salary:f2}\n";
            }
            return result;
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context) // Task - 0.4
        {
            StringBuilder sb = new StringBuilder();
            var employees = context.Employees
                .Where(x => x.Salary > 50000)
                .Select(x => new { x.FirstName, x.Salary })
                .OrderBy(x => x.FirstName)
                .ToArray();

            foreach (var person in employees)
            {
                sb.AppendLine($"{person.FirstName} - {person.Salary:f2}");
            }
            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context) // Task - 0.5
        {
            StringBuilder sb = new StringBuilder();
            var employees = context.Employees
                .Where(x => x.Department.Name == "Research and Development")
                .Select(x => new
                {
                    firstName = x.FirstName,
                    lastName = x.LastName,
                    deppName = x.Department.Name,
                    salary = x.Salary,
                })
                .OrderBy(x => x.salary)
                .ThenByDescending(x => x.firstName)
                .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.firstName} {e.lastName} from {e.deppName} - ${e.salary:f2}");
            }
            return sb.ToString().TrimEnd();
        }

        public static string AddNewAddressToEmployee(SoftUniContext context) // Task - 0.6
        {
            var nakovTheBoss = context.Employees.First(x => x.LastName == "Nakov");
            var nakovNewHome = new Address
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };
            context.Addresses.Add(nakovNewHome);
            nakovTheBoss.Address = nakovNewHome;

            context.SaveChanges();


            StringBuilder sb = new StringBuilder();
            var employees = context.Employees
                .OrderByDescending(x => x.AddressId)
                .Take(10)
                .Select(x => x.Address.AddressText)
                .ToArray();

            return string.Join('\n', employees);
        }


    }
}
