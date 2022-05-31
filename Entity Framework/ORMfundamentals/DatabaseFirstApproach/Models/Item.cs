namespace DatabaseFirstApproach.Models
{
    public partial class Item
    {
        public Item()
        {
            GameTypes = new HashSet<GameType>();
            UserGames = new HashSet<UsersGame>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public int ItemTypeId { get; set; }
        public int StatisticId { get; set; }
        public decimal Price { get; set; }
        public int MinLevel { get; set; }

        public virtual ItemType ItemType { get; set; } = null!;
        public virtual Statistic Statistic { get; set; } = null!;

        public virtual ICollection<GameType> GameTypes { get; set; }
        public virtual ICollection<UsersGame> UserGames { get; set; }
    }
}
