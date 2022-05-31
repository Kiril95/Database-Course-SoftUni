using DatabaseFirstApproach.Models;

public class Program
{
    static void Main()
    {
        // Inside .Net 6, in order to successfully scaffold our database we have to write our own Main method :] 

        var db = new DiabloContext();
        db.Characters.Add(new Character { Name = "Shaaman" });
        db.SaveChanges();

        Console.WriteLine(db.Characters.Count());


        var gameTypes = db.GameTypes
            .Where(x => x.BonusStatsId > 100)
            .OrderByDescending(x => x.BonusStatsId)
            .Select(x => new { difficulty = x.Name, statsId = x.BonusStatsId })
            .ToList();

        gameTypes.ForEach(info => Console.WriteLine($"StatsId - {info.statsId} <> Difficulty - {info.difficulty}"));


        var characters = db.Characters
            .GroupBy(x => x.Name)
            .Select(x => new { @class = x.Key, count = x.Count() })
            .ToList();

        characters.ForEach(info => Console.WriteLine($"Class - {info.@class} <> Count - {info.count}"));


        var items = from i in db.Items
                    where i.Price > 700
                    orderby i.Name descending
                    select new { name = i.Name, itemLevel = i.MinLevel, price = i.Price };

        foreach (var item in items)
        {
            Console.WriteLine($"Name - {item.name} <> Level - {item.itemLevel} <> Price - {item.price:f2}");
        }
    }
}
