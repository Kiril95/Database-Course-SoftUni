using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TeisterMask.Data.Models
{
    public class Employee
    {
        public Employee()
        {
            this.EmployeesTasks = new HashSet<EmployeeTask>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 3)]
        [RegularExpression(@"^\w+$")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^\d{3}-\d{3}-\d{4}$")]
        public string Phone { get; set; }

        public virtual ICollection<EmployeeTask> EmployeesTasks { get; set; }
    }
}
