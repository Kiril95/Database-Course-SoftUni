using System.ComponentModel.DataAnnotations;

namespace TeisterMask.DataProcessor.ImportDto
{
    public class EmployeesImportDTO
    {
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

        public int[] Tasks { get; set; }
    }
}
