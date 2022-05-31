namespace DatabaseFirstApproach.Models
{
    public partial class Game
    {
        public Game()
        {
            UsersGames = new HashSet<UsersGame>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime Start { get; set; }
        public int? Duration { get; set; }
        public int GameTypeId { get; set; }
        public bool IsFinished { get; set; }

        public virtual GameType GameType { get; set; } = null!;
        public virtual ICollection<UsersGame> UsersGames { get; set; }
    }
}
