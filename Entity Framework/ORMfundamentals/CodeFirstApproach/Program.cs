using CodeFirstApproach.Models;

public class Program
{
    static void Main()
    {
        var db = new FootballDbContext();
        db.Database.EnsureCreated();

    }
}
