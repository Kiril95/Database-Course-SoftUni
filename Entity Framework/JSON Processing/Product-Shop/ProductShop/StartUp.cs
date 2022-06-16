using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.DTO;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var db = new ProductShopContext();
            //db.Database.EnsureCreated();

            // 1. Import data
            //string usersJson = File.ReadAllText("../../../Datasets/users.json");
            //string productsJson = File.ReadAllText("../../../Datasets/products.json");
            //string categoriesJson = File.ReadAllText("../../../Datasets/categories.json");
            //string categoryProductsJson = File.ReadAllText("../../../Datasets/categories-products.json");

            //Console.WriteLine(ImportCategoryProducts(db, categoryProductsJson));


            // 2. Export data
            //File.WriteAllText("../../../Datasets/products-in-range.json", GetProductsInRange(db));
            //File.WriteAllText("../../../Datasets/users-sold-products.json", GetSoldProducts(db));
            //File.WriteAllText("../../../Datasets/categories-by-products.json", GetCategoriesByProductsCount(db));
            File.WriteAllText("../../../Datasets/users-and-products.json", GetUsersWithProducts(db));

            string result = GetUsersWithProducts(db);
            Console.WriteLine(result);
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)  // Task 0.1
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });
            IMapper mapper = new Mapper(config);

            var deserialize = JsonConvert.DeserializeObject<IEnumerable<UserDTO>>(inputJson);

            var users = mapper.Map<IEnumerable<User>>(deserialize);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count()}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)  // Task 0.2
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });
            IMapper mapper = new Mapper(config);

            var deserialize = JsonConvert.DeserializeObject<IEnumerable<ProductDTO>>(inputJson);

            var products = mapper.Map<IEnumerable<Product>>(deserialize);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count()}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)  // Task 0.3
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });
            IMapper mapper = new Mapper(config);

            var deserialize = JsonConvert.DeserializeObject<IEnumerable<CategoryDTO>>(inputJson)
                .Where(x => x.Name != null);

            var categories = mapper.Map<IEnumerable<Category>>(deserialize);

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count()}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)  // Task 0.4
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });
            IMapper mapper = new Mapper(config);

            var deserialize = JsonConvert.DeserializeObject<IEnumerable<CategoryProductDTO>>(inputJson);

            var catalog = mapper.Map<IEnumerable<CategoryProduct>>(deserialize);

            context.CategoryProducts.AddRange(catalog);
            context.SaveChanges();

            return $"Successfully imported {catalog.Count()}";
        }

        public static string GetProductsInRange(ProductShopContext context)  // Task 0.5
        {
            var products = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .Select(x => new
                {
                    Name = x.Name,
                    Price = x.Price,
                    Seller = $"{x.Seller.FirstName} {x.Seller.LastName}"
                })
                .OrderBy(x => x.Price)
                .ToArray();

            DefaultContractResolver resolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var settings = new JsonSerializerSettings()
            {
                ContractResolver = resolver,
                Formatting = Formatting.Indented,
            };

            return JsonConvert.SerializeObject(products, settings);
        }

        public static string GetSoldProducts(ProductShopContext context)  // Task 0.6
        {
            var users = context.Users
                .Where(x => x.ProductsSold.Any(p => p.BuyerId != null))
                .Select(x => new
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    SoldProducts = x.ProductsSold.Select(p => new
                    {
                        Name = p.Name,
                        Price = p.Price,
                        BuyerFirstName = p.Buyer.FirstName,
                        BuyerLastName = p.Buyer.LastName,
                    })
                })
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .ToArray();

            DefaultContractResolver resolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var settings = new JsonSerializerSettings()
            {
                ContractResolver = resolver,
                Formatting = Formatting.Indented,
            };

            return JsonConvert.SerializeObject(users, settings);
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)  // Task 0.7
        {
            var categories = context.Categories
                .OrderByDescending(x => x.CategoryProducts.Count())
                .Select(x => new
                {
                    Category = x.Name,
                    ProductsCount = x.CategoryProducts.Count(),
                    AveragePrice = x.CategoryProducts.Average(p => p.Product.Price).ToString("f2"),
                    TotalRevenue = x.CategoryProducts.Sum(p => p.Product.Price).ToString("f2")
                })
                .ToArray();

            DefaultContractResolver resolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var settings = new JsonSerializerSettings()
            {
                ContractResolver = resolver,
                Formatting = Formatting.Indented,
            };

            return JsonConvert.SerializeObject(categories, settings);
        }

        public static string GetUsersWithProducts(ProductShopContext context)  // Task 0.8
        {
            var users = context.Users
                .Include(x => x.ProductsSold)
                .ToArray()
                .Where(x => x.ProductsSold.Any(p => p.BuyerId != null))
                .Select(x => new
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Age = x.Age,
                    SoldProducts = new
                    {
                        Count = x.ProductsSold.Where(p => p.BuyerId != null).Count(),
                        Products = x.ProductsSold.Where(p => p.BuyerId != null).Select(p => new
                        {
                            Name = p.Name,
                            Price = p.Price
                        })
                        .ToArray()
                    }
                })
                .OrderByDescending(x => x.SoldProducts.Count);

            var result = new
            {
                UsersCount = users.Count(),
                Users = users
            };

            DefaultContractResolver resolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var settings = new JsonSerializerSettings()
            {
                ContractResolver = resolver,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
            };

            return JsonConvert.SerializeObject(result, settings);
        }
    }
}