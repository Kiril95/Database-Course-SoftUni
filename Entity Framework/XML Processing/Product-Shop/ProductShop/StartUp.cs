using AutoMapper;
using ProductShop.Data;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var db = new ProductShopContext();
            //db.Database.EnsureCreated();

            // 1. Import data
            //string usersXml = File.ReadAllText("../../../Datasets/users.xml");
            //string productsXml = File.ReadAllText("../../../Datasets/products.xml");
            //string categoriesXml = File.ReadAllText("../../../Datasets/categories.xml");
            //string categoriesProductsXml = File.ReadAllText("../../../Datasets/categories-products.xml");

            //Console.WriteLine(ImportCategoryProducts(db, categoriesProductsXml));


            // 2. Export data
            //File.WriteAllText("../../../Datasets/products-in-range.xml", GetProductsInRange(db));
            //File.WriteAllText("../../../Datasets/users-sold-products.xml", GetSoldProducts(db));
            //File.WriteAllText("../../../Datasets/categories-by-products.xml", GetCategoriesByProductsCount(db));
            File.WriteAllText("../../../Datasets/users-and-products.xml", GetUsersWithProducts(db));

            string result = GetUsersWithProducts(db);
            Console.WriteLine(result);
        }

        public static string ImportUsers(ProductShopContext context, string inputXml) // Task 0.1
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });
            IMapper mapper = new Mapper(config);

            XmlSerializer serializer = new XmlSerializer(typeof(UserDTO), new XmlRootAttribute("Users"));
            StringReader reader = new StringReader(inputXml);

            var deserialize = (UserDTO[])serializer.Deserialize(reader);

            var users = mapper.Map<IEnumerable<User>>(deserialize);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count()}";
        }

        public static string ImportProducts(ProductShopContext context, string inputXml) // Task 0.2
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });
            IMapper mapper = new Mapper(config);

            XmlSerializer serializer = new XmlSerializer(typeof(ProductDTO[]), new XmlRootAttribute("Products"));
            StringReader reader = new StringReader(inputXml);

            var deserialize = (ProductDTO[])serializer.Deserialize(reader);

            var products = mapper.Map<IEnumerable<Product>>(deserialize);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count()}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml) // Task 0.3
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });
            IMapper mapper = new Mapper(config);

            XmlSerializer serializer = new XmlSerializer(typeof(CategoryDTO[]), new XmlRootAttribute("Categories"));
            StringReader reader = new StringReader(inputXml);

            var deserialize = (CategoryDTO[])serializer.Deserialize(reader);
            var filtered = deserialize.Where(x => x.Name != null).ToArray();

            var categories = mapper.Map<IEnumerable<Category>>(filtered);

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count()}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml) // Task 0.4
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });
            IMapper mapper = new Mapper(config);

            XmlSerializer serializer = new XmlSerializer(typeof(CategoryProductDTO[]), new XmlRootAttribute("CategoryProducts"));
            StringReader reader = new StringReader(inputXml);

            var categoriesIds = context.Categories.Select(x => x.Id).ToList();
            var productsIds = context.Products.Select(x => x.Id).ToList();

            var deserialize = (CategoryProductDTO[])serializer.Deserialize(reader);
            var filtered = deserialize
                .Where(x => categoriesIds.Contains(x.CategoryId) && productsIds.Contains(x.ProductId))
                .ToArray();

            var collection = mapper.Map<IEnumerable<CategoryProduct>>(filtered);

            context.CategoryProducts.AddRange(collection);
            context.SaveChanges();

            return $"Successfully imported {collection.Count()}";
        }

        public static string GetProductsInRange(ProductShopContext context)  // Task 0.5
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(ProductsInRangeDTO[]), new XmlRootAttribute("Products"));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            StringWriter writer = new StringWriter(sb);

            var products = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .Select(p => new ProductsInRangeDTO
                {
                    Name = p.Name,
                    Price = p.Price,
                    Buyer = $"{p.Buyer.FirstName} {p.Buyer.LastName}",
                })
                .OrderBy(x => x.Price)
                .Take(10)
                .ToArray();

            serializer.Serialize(writer, products, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetSoldProducts(ProductShopContext context)  // Task 0.6
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(UserProductDTO[]), new XmlRootAttribute("Users"));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            StringWriter writer = new StringWriter(sb);

            var users = context.Users
                .Where(x => x.ProductsSold.Count() >= 1)
                .Select(x => new UserProductDTO
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    SoldProducts = x.ProductsSold.Select(p => new SoldProductDTO
                    {
                        Name = p.Name,
                        Price = p.Price
                    })
                    .ToArray()
                })
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Take(5)
                .ToArray();

            serializer.Serialize(writer, users, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)  // Task 0.7
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(CategoriesByProductCountDTO[]), new XmlRootAttribute("Categories"));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            StringWriter writer = new StringWriter(sb);

            var categories = context.Categories
                .Select(x => new CategoriesByProductCountDTO
                {
                    Name = x.Name,
                    Count = x.CategoryProducts.Count(),
                    AveragePrice = x.CategoryProducts.Average(p => p.Product.Price),
                    TotalRevenue = x.CategoryProducts.Sum(p => p.Product.Price)
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.TotalRevenue)
                .ToArray();

            serializer.Serialize(writer, categories, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetUsersWithProducts(ProductShopContext context)  // Task 0.8
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(ExportModel), new XmlRootAttribute("Users"));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            StringWriter writer = new StringWriter(sb);

            var users = context.Users
                //.ToArray()  This is needed for the 100 points in Judge... but with this we aren't getting any users serialized in the file :(
                .Where(x => x.ProductsSold.Count >= 1)
                .Select(x => new UsersWithProductsDTO
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Age = x.Age,
                    SoldProducts = new UserSoldProduct
                    {
                        Count = x.ProductsSold.Count(),
                        Products = x.ProductsSold.Select(p => new SoldProductDTO
                        {
                            Name = p.Name,
                            Price = p.Price
                        })
                        .OrderByDescending(p => p.Price)
                        .ToArray()
                    }
                })
                .OrderByDescending(x => x.SoldProducts.Count)
                .Take(10)
                .ToArray();

            var result = new ExportModel
            {
                Count = context.Users.Where(x => x.ProductsSold.Any()).Count(),
                Users = users
            };

            serializer.Serialize(writer, result, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}