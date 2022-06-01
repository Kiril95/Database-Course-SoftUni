using Microsoft.EntityFrameworkCore;

namespace CodeFirstApproach.Models
{
    public class FootballDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=Football;Integrated Security=true;Encrypt=False;");
        }

        public DbSet<Country>? Countries { get; set; }

        public DbSet<League>? Leagues { get; set; }

        public DbSet<Team>? Teams { get; set; }

        public DbSet<Player>? Players { get; set; }


    }
}
