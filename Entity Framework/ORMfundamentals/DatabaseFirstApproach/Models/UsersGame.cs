namespace DatabaseFirstApproach.Models
{
    public partial class UsersGame
    {
        public UsersGame()
        {
            Items = new HashSet<Item>();
        }

        public int Id { get; set; }
        public int GameId { get; set; }
        public int UserId { get; set; }
        public int CharacterId { get; set; }
        public int Level { get; set; }
        public DateTime JoinedOn { get; set; }
        public decimal Cash { get; set; }

        public virtual Character Character { get; set; } = null!;
        public virtual Game Game { get; set; } = null!;
        public virtual User User { get; set; } = null!;

        public virtual ICollection<Item> Items { get; set; }
    }
}
