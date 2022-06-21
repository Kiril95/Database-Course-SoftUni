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
            //dbContext.Database.EnsureCreated();

            // 1. Import data
            //string usersXml = File.ReadAllText("../../../Datasets/users.xml");
            //string productsXml = File.ReadAllText("../../../Datasets/products.xml");
            //string categoriesXml = File.ReadAllText("../../../Datasets/categories.xml");
            //string categoriesProductsXml = File.ReadAllText("../../../Datasets/categories-products.xml");

            //Console.WriteLine(ImportCategoryProducts(db, categoriesProductsXml));


            // 2. Export data
            //File.WriteAllText("../../../Datasets/products-in-range.xml", GetProductsInRange(db));

            string result = GetProductsInRange(db);
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

            XmlSerializer serializer = new XmlSerializer(typeof(CategoryProductDTO[]), new XmlRootAttribute("CategoryProducts"));
            StringWriter writer = new StringWriter(sb);

            var products = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .Select(p => new ProductsInRangeDTO
                {
                    Name = p.Name,
                    Price = p.Price,
                    Buyer = p.Buyer.FirstName + " " + p.Buyer.LastName,
                })
                .OrderBy(x => x.Price)
                .Take(10)
                .ToArray();

            serializer.Serialize(writer, products);

            return sb.ToString().TrimEnd();
        }


    }
}