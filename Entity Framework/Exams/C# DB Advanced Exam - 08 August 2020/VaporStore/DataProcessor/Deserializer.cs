namespace VaporStore.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.Dto.Import;

    public static class Deserializer
    {
        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var deserialize = JsonConvert.DeserializeObject<JsonGamesImportDTO[]>(jsonString);

            foreach (var gameItem in deserialize)
            {
                if (!IsValid(gameItem))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                DateTime releaseDate;
                bool checkReleaseDate = DateTime.TryParseExact(gameItem.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out releaseDate);

                if (!checkReleaseDate)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                // Check if there is a genre with given name and if there isn't, create a new one
                var genre = context.Genres.FirstOrDefault(g => g.Name == gameItem.Genre) ?? new Genre { Name = gameItem.Genre };
                var developer = context.Developers.FirstOrDefault(d => d.Name == gameItem.Developer) ?? new Developer { Name = gameItem.Developer };

                Game game = new Game()
                {
                    Name = gameItem.Name,
                    Price = gameItem.Price,
                    ReleaseDate = releaseDate,
                    Genre = genre,
                    Developer = developer
                };

                foreach (var tagItem in gameItem.Tags)
                {
                    if (!IsValid(tagItem))
                    {
                        sb.AppendLine("Invalid Data");
                        continue;
                    }
                    var tag = context.Tags.FirstOrDefault(t => t.Name == tagItem) ?? new Tag { Name = tagItem };

                    game.GameTags.Add(new GameTag()
                    {
                        Tag = tag
                    });

                }
                context.Games.Add(game);
                context.SaveChanges();

                sb.AppendLine($"Added {game.Name} ({game.Genre.Name}) with {game.GameTags.Count()} tags");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var deserialize = JsonConvert.DeserializeObject<JsonUsersImportDTO[]>(jsonString);

            foreach (var userItem in deserialize)
            {
                if (!IsValid(userItem))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                User user = new User()
                {
                    FullName = userItem.FullName,
                    Username = userItem.Username,
                    Email = userItem.Email,
                    Age = userItem.Age,
                };

                foreach (var cardItem in userItem.Cards)
                {
                    if (!IsValid(cardItem))
                    {
                        sb.AppendLine("Invalid Data");
                        continue;
                    }

                    CardType type;
                    var check = Enum.TryParse(cardItem.Type, out type);

                    user.Cards.Add(new Card
                    {
                        Number = cardItem.Number,
                        Cvc = cardItem.CVC,
                        Type = type
                    });

                }
                context.Users.Add(user);
                context.SaveChanges();

                sb.AppendLine($"Imported {user.Username} with {user.Cards.Count()} cards");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            List<Purchase> purchases = new List<Purchase>();

            XmlSerializer serializer = new XmlSerializer(typeof(XmlPurchasesImportDTO[]), new XmlRootAttribute("Purchases"));
            using StringReader reader = new StringReader(xmlString);

            var deserialize = (XmlPurchasesImportDTO[])serializer.Deserialize(reader);

            foreach (var purchaseItem in deserialize)
            {
                if (!IsValid(purchaseItem))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                PurchaseType type;
                var checkType = Enum.TryParse(purchaseItem.Type, out type);
                if (!checkType)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                DateTime date;
                var checkDate = DateTime.TryParseExact(purchaseItem.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out date);
                if (!checkDate)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Purchase purchase = new Purchase()
                {
                    Game = context.Games.FirstOrDefault(x => x.Name == purchaseItem.Title),
                    Type = type,
                    ProductKey = purchaseItem.Key,
                    Card = context.Cards.FirstOrDefault(x => x.Number == purchaseItem.Card),
                    Date = date
                };

                purchases.Add(purchase);
                sb.AppendLine($"Imported {purchase.Game.Name} for {purchase.Card.User.Username}");
            }

            context.Purchases.AddRange(purchases);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}