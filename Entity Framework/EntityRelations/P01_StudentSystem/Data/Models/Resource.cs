using System.ComponentModel.DataAnnotations;

namespace P01_StudentSystem.Data.Models
{
    public class Resource
    {
        [Key]
        public int ResourceId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public string Url { get; set; }

        public virtual ResourceType ResourceType { get; set; }

        public int CourseId { get; set; }

        public virtual Course Course { get; set; }
    }
}
