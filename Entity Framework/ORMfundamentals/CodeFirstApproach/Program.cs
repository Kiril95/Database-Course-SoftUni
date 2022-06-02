using CodeFirstApproach.Models;

public class Program
{
    static void Main()
    {
        var db = new FootballDbContext();
        db.Database.EnsureCreated();

        db.Countries?.Add(new Country
        {
            Name = "Bulgaria",
            TeamColor = "White and green",
            NationalStadium = "Vasil Levski",
            Leagues = new List<League>
            {
                new League
                {
                    Name = "Efbet League",
                    Teams = new List<Team>
                    {
                        new Team
                        {
                            Name = "Levski",
                            TeamColor = "Blue",
                            ClubStadium = "Georgi Asparuhov",
                            Owner = "Nasko Sirakov",
                            Coach = "Stanimir Stoilov",
                            Titles = 26,
                            Players = new List<Player>
                            {
                                new Player
                                {
                                    Name = "Georgi Milanov",
                                    Age = 30,
                                    Position = "Midfielder"
                                }
                            }
                        }
                    }

                }
            }
        });

        //////////////////////////////////////////////////////////////////////////////////////////////////////////

        // At this point I've already created the other data for the English teams
        // This finds the entity that we need to get the Id from, in order to add the new team to the right League
        var leagueEntity = db.Leagues?.First(x => x.Name == "Premier League");

        db.Teams?.Add(new Team
        {
            Name = "Chelsea",
            LeagueId = leagueEntity!.Id,
            TeamColor = "Blue",
            ClubStadium = "Stamford Bridge",
            Owner = "Roman Abramovich",
            Coach = "Thomas Tuchel",
            Titles = 6,
            Players = new List<Player>
            {
                new Player
                {
                    Name = "Cesar Azpilicueta",
                    Age = 32,
                    Position = "Defender"
                }
            }
        });

        /////////////////////////////////////////////////////////////////////////////////////////////////////////

        Team teamEntity = db.Teams!.First(x => x.Name == "Levski");

        db.Players?.Add(new Player
        {
            Name = "Martin Petkov",
            Age = 18,
            Position = "Attacker",
            TeamId = teamEntity!.Id
        });


        db.SaveChanges();
    }
}
