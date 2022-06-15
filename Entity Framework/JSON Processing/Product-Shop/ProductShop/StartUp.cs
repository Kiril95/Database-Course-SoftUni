using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
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

            //string usersJson = File.ReadAllText("../../../Datasets/users.json");
            //string productsJson = File.ReadAllText("../../../Datasets/products.json");
            //string categoriesJson = File.ReadAllText("../../../Datasets/categories.json");
            string categoryProductsJson = File.ReadAllText("../../../Datasets/categories-products.json");

            Console.WriteLine(categoryProductsJson);
            Console.WriteLine(ImportCategoryProducts(db, categoryProductsJson));
        }

        public static string ImportUsers(ProductShopContext context, string inputJson) // Task 0.2
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

        public static string ImportProducts(ProductShopContext context, string inputJson) // Task 0.3
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

        public static string ImportCategories(ProductShopContext context, string inputJson) // Task 0.4
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

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson) // Task 0.5
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




    }
}