namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";

        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            List<Play> plays = new List<Play>();

            XmlSerializer serializer = new XmlSerializer(typeof(PlayImportDTO[]), new XmlRootAttribute("Plays"));
            using StringReader reader = new StringReader(xmlString);

            var deserialize = (PlayImportDTO[])serializer.Deserialize(reader);

            foreach (var playItem in deserialize)
            {
                if (!IsValid(playItem))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                TimeSpan duration = TimeSpan.ParseExact(playItem.Duration, "c", CultureInfo.InvariantCulture);
                if (duration.TotalHours < 1)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Genre genre;
                bool check = Enum.TryParse(playItem.Genre, out genre);
                if (!check)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Play play = new Play()
                {
                    Title = playItem.Title,
                    Duration = duration,
                    Rating = playItem.Rating,
                    Genre = genre,
                    Description = playItem.Description,
                    Screenwriter = playItem.Screenwriter
                };

                plays.Add(play);
                sb.AppendLine(string.Format(SuccessfulImportPlay, play.Title, play.Genre, play.Rating));
            }

            context.Plays.AddRange(plays);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            List<Cast> casts = new List<Cast>();

            XmlSerializer serializer = new XmlSerializer(typeof(CastImportDTO[]), new XmlRootAttribute("Casts"));
            using StringReader reader = new StringReader(xmlString);

            var deserialize = (CastImportDTO[])serializer.Deserialize(reader);

            foreach (var castItem in deserialize)
            {
                if (!IsValid(castItem))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                string type = castItem.IsMainCharacter == true ? "main" : "lesser";

                Cast cast = new Cast()
                {
                    FullName = castItem.FullName,
                    IsMainCharacter = castItem.IsMainCharacter,
                    PhoneNumber = castItem.PhoneNumber,
                    PlayId = castItem.PlayId
                };

                casts.Add(cast);
                sb.AppendLine(string.Format(SuccessfulImportActor, cast.FullName, type));
            }

            context.Casts.AddRange(casts);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            List<Theatre> theatres = new List<Theatre>();
            List<Ticket> tickets = new List<Ticket>();

            var deserialize = JsonConvert.DeserializeObject<IEnumerable<TheatreImportDTO>>(jsonString);

            foreach (var theatreItem in deserialize)
            {
                if (!IsValid(theatreItem))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Theatre theatre = new Theatre()
                {
                    Name = theatreItem.Name,
                    NumberOfHalls = theatreItem.NumberOfHalls,
                    Director = theatreItem.Director,
                };

                foreach (var ticketItem in theatreItem.Tickets)
                {
                    if (!IsValid(ticketItem))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Ticket ticket = new Ticket
                    {
                        Price = ticketItem.Price,
                        RowNumber = ticketItem.RowNumber,
                        PlayId = ticketItem.PlayId
                    };

                    tickets.Add(ticket);
                    theatre.Tickets.Add(ticket);
                }

                theatres.Add(theatre);
                sb.AppendLine(string.Format(SuccessfulImportTheatre, theatre.Name, theatre.Tickets.Count()));
            }

            context.Theatres.AddRange(theatres);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }


        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
