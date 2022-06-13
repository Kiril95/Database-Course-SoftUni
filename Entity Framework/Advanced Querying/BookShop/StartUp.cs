namespace BookShop
{
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Microsoft.EntityFrameworkCore;
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
            //string input = Console.ReadLine();

            var exec = RemoveBooks(db);
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

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)   // Task 0.8
        {
            var authors = context.Authors
                .ToArray()
                .Where(x => x.FirstName.EndsWith(input))
                .Select(x => new
                {
                    FullName = $"{x.FirstName} {x.LastName}",
                })
                .OrderBy(x => x.FullName);

            return string.Join("\n", authors.Select(x => x.FullName));
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)   // Task 0.9
        {
            var books = context.Books
                .ToArray()
                .Where(x => x.Title.Contains(input, StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Title)
                .OrderBy(x => x);

            return string.Join("\n", books);
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)   // Task 10
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .Include(x => x.Author)
                .ToArray()
                .Where(x => x.Author.LastName.StartsWith(input, StringComparison.OrdinalIgnoreCase))
                .OrderBy(x => x.BookId)
                .Select(x => new
                {
                    Title = x.Title,
                    AuthorName = $"{x.Author.FirstName} {x.Author.LastName}",
                });

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} ({book.AuthorName})");
            }

            return sb.ToString().TrimEnd();
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)   // Task 11
        {
            return context.Books.Where(x => x.Title.Length > lengthCheck).Count();
        }

        public static string CountCopiesByAuthor(BookShopContext context)   // Task 12
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Authors
                .Select(x => new
                {
                    AuthorName = $"{x.FirstName} {x.LastName}",
                    TotalCopies = x.Books.Sum(x => x.Copies)
                })
                .OrderByDescending(x => x.TotalCopies)
                .ToArray();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.AuthorName} - {book.TotalCopies}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)   // Task 13
        {
            StringBuilder sb = new StringBuilder();

            var categories = context.Categories
                .Select(x => new
                {
                    Category = x.Name,
                    Profit = x.CategoryBooks.Sum(x => x.Book.Copies * x.Book.Price)
                })
                .OrderByDescending(x => x.Profit)
                .ThenBy(x => x.Category)
                .ToArray();

            foreach (var book in categories)
            {
                sb.AppendLine($"{book.Category} ${book.Profit:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetMostRecentBooks(BookShopContext context)   // Task 14
        {
            StringBuilder sb = new StringBuilder();

            var categories = context.Categories
                .Select(x => new
                {
                    CategoryName = x.Name,
                    Books = x.CategoryBooks.Select(b => new
                    {
                        BookName = b.Book.Title,
                        ReleaseDate = b.Book.ReleaseDate
                    })
                    .OrderByDescending(x => x.ReleaseDate)
                    .Take(3)
                })
                .OrderBy(x => x.CategoryName)
                .ToArray();

            foreach (var category in categories)
            {
                sb.AppendLine($"--{category.CategoryName}");

                foreach (var book in category.Books)
                {
                    sb.AppendLine($"{book.BookName} ({book.ReleaseDate.Value.Year})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static void IncreasePrices(BookShopContext context)   // Task 15
        {
            context.Books
               .Where(x => x.ReleaseDate.Value.Year < 2010)
               .ToList()
               .ForEach(b => b.Price += 5);
        }

        public static int RemoveBooks(BookShopContext context)   // Task 16
        {
            var booksForDeletion = context.Books.Where(x => x.Copies < 4200).ToArray();

            //context.Books.RemoveRange(context.Books.Where(x => x.Copies < 4200));  Alternative
            context.Books.RemoveRange(booksForDeletion);
            context.SaveChanges();

            return booksForDeletion.Count();
        }
    }
}
