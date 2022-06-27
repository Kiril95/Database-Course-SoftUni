using System;
using System.ComponentModel.DataAnnotations;

namespace VaporStore.DataProcessor.Dto.Import
{
    public class JsonUsersImportDTO
    {
        [Required]
        [RegularExpression(@"^[A-Z]{1}[a-z]+\s{1}[A-Z]{1}[a-z]+$")]
        public string FullName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Range(3, 103)]
        public int Age { get; set; }

        public JsonCardsImportDTO[] Cards { get; set; }
    }

    public class JsonCardsImportDTO
    {
        [Required]
        [RegularExpression(@"^\d{4} \d{4} \d{4} \d{4}$")]
        public string Number { get; set; }

        [Required]
        [RegularExpression(@"^\d{3}$")]
        public string CVC { get; set; }

        [Required]
        public string Type { get; set; }
    }
}
