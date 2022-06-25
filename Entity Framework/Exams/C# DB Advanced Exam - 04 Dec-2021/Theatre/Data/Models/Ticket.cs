using System.ComponentModel.DataAnnotations;

namespace Theatre.Data.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [Range(1.00, 100.00)]
        public decimal Price { get; set; }

        [Range(1, 10)]
        public sbyte RowNumber { get; set; }

        public int PlayId { get; set; }
        public virtual Play Play { get; set; }

        public int TheatreId { get; set; }
        public virtual Theatre Theatre { get; set; }
    }
}