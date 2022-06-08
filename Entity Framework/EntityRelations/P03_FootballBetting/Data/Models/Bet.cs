using System;
using System.ComponentModel.DataAnnotations;

namespace P03_FootballBetting.Data.Models
{
    public class Bet
    {
        [Key]
        public int BetId { get; set; }

        [Range(10, 500000)]
        public decimal Amount { get; set; }

        [Required]
        public PredictionType Prediction { get; set; }

        public DateTime DateTime { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int GameId { get; set; }
        public virtual Game Game { get; set; }
    }
}
