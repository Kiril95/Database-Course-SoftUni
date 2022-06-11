namespace BookShop
{
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            //int input = int.Parse(Console.ReadLine());
            string input = Console.ReadLine();

            var exec = GetBooksReleasedBefore(db, input);
            Console.WriteLine(exec);
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command) // Task 0.2
        {
            var ageClause = Enum.Parse<AgeRestriction>(command, true);

            var books = context.Books
                .ToArray()
                .Where(x => x.AgeRestriction == ageClause)
                .Select(b => b.Title)
                .OrderBy(b => b);

            return string.Join("\n", books);
        }

        public static string GetGoldenBooks(BookShopContext context)  // Task 0.3
        {
            var books = context.Books
                .ToArray()
                .Where(x => x.EditionType == EditionType.Gold && x.Copies < 5000)
                .OrderBy(x => x.BookId)
                .Select(b => b.Title);

            return string.Join("\n", books);
        }

        public static string GetBooksByPrice(BookShopContext context)   // Task 0.4
        {
            StringBuilder sb = new StringBuilder();

            var titles = context.Books
                .ToArray()
                .Where(x => x.Price > 40)
                .OrderByDescending(x => x.Price)
                .Select(b => new
                {
                    Title = b.Title,
                    Price = b.Price
                });

            foreach (var book in titles)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)   // Task 0.5
        {
            IEnumerable<string> books = context.Books
                .ToArray()
                .Where(x => x.ReleaseDate.Value.Year != year)
                .OrderBy(x => x.BookId)
                .Select(b => b.Title);

            return string.Join("\n", books);
        }

        public static string GetBooksByCategory(BookShopContext context, string input)   // Task 0.6
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<string> titles = context.Books
                .Where(x => x.BookCategories.Any(c => input.Trim().ToLower().Contains(c.Category.Name.ToLower())))
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToArray();

            foreach (var book in titles)
            {
                sb.AppendLine($"{book}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)   // Task 0.7
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .Where(x => x.ReleaseDate.Value < DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture))
                .Select(x => new
                {
                    Title = x.Title,
                    Price = x.Price,
                    Edition = x.EditionType,
                    ReleaseDate = x.ReleaseDate.Value
                })
                .OrderByDescending(x => x.ReleaseDate)
                .ToArray();


            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.Edition} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }



    }
}
