using System;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    public class JsonPrisonerImportDTO
    {
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string FullName { get; set; }

        [Required]
        [RegularExpression(@"^The [A-Z]{1}[a-z]+$")]
        public string Nickname { get; set; }

        [Range(18, 65)]
        public int Age { get; set; }

        [Required]
        public string IncarcerationDate { get; set; }

        public string ReleaseDate { get; set; }

        [Range(0.00, double.MaxValue)]
        public decimal? Bail { get; set; }

        public int? CellId { get; set; }

        public JsonMailImportDTO[] Mails { get; set; }
    }

    public class JsonMailImportDTO
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public string Sender { get; set; }

        [Required]
        [RegularExpression(@"[\w\s]+str.")]
        public string Address { get; set; }
    }
}
