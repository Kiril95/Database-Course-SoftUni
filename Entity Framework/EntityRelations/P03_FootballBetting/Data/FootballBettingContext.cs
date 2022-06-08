using Microsoft.EntityFrameworkCore;
using P03_FootballBetting.Data.Models;

namespace P03_FootballBetting.Data
{
    public class FootballBettingContext : DbContext
    {
        public FootballBettingContext()
        {
        }

        public FootballBettingContext(DbContextOptions<FootballBettingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bet> Bets { get; set; }

        public virtual DbSet<Color> Colors { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Game> Games { get; set; }

        public virtual DbSet<Player> Players { get; set; }

        public virtual DbSet<Position> Positions { get; set; }

        public virtual DbSet<Town> Towns { get; set; }

        public virtual DbSet<Country> Countries { get; set; }

        public virtual DbSet<Team> Teams { get; set; }

        public virtual DbSet<PlayerStatistic> PlayerStatistics { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=FootballBookmakerSystem;Integrated Security=true;Encrypt=False;");
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasAlternateKey(x => new { x.Username, x.Email });

            modelBuilder.Entity<PlayerStatistic>().HasKey(x => new
            {
                x.PlayerId,
                x.GameId
            });

            modelBuilder.Entity<Team>()
                .HasOne(pk => pk.PrimaryKitColor)
                .WithMany(pk => pk.PrimaryKitTeams)
                .HasForeignKey(pk => pk.PrimaryKitColorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Team>()
                .HasOne(sk => sk.SecondaryKitColor)
                .WithMany(sk => sk.SecondaryKitTeams)
                .HasForeignKey(sk => sk.SecondaryKitColorId);

            modelBuilder.Entity<Team>()
                .HasOne(t => t.Town)
                .WithMany(t => t.Teams)
                .HasForeignKey(t => t.TownId);

            modelBuilder.Entity<Game>()
                .HasOne(ht => ht.HomeTeam)
                .WithMany(hg => hg.HomeGames)
                .HasForeignKey(ht => ht.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Game>()
                .HasOne(at => at.AwayTeam)
                .WithMany(ag => ag.AwayGames)
                .HasForeignKey(at => at.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Town>()
                .HasOne(c => c.Country)
                .WithMany(t => t.Towns)
                .HasForeignKey(c => c.CountryId);

            modelBuilder.Entity<Player>()
                .HasOne(t => t.Team)
                .WithMany(p => p.Players)
                .HasForeignKey(t => t.TeamId);

            modelBuilder.Entity<Player>()
                .HasOne(t => t.Team)
                .WithMany(p => p.Players)
                .HasForeignKey(t => t.TeamId);

            modelBuilder.Entity<Player>()
                .HasOne(p => p.Position)
                .WithMany(p => p.Players)
                .HasForeignKey(p => p.PositionId);

            modelBuilder.Entity<Bet>()
                .HasOne(g => g.Game)
                .WithMany(b => b.Bets)
                .HasForeignKey(g => g.GameId);

            modelBuilder.Entity<Bet>()
               .HasOne(u => u.User)
               .WithMany(b => b.Bets)
               .HasForeignKey(u => u.UserId);
        }
    }
}
