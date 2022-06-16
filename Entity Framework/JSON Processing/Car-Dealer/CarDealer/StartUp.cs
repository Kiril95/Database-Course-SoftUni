using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var db = new CarDealerContext();
            //db.Database.EnsureCreated();

            // 1. Import data
            //string suppliersJson = File.ReadAllText("../../../Datasets/suppliers.json");
            //string partsJson = File.ReadAllText("../../../Datasets/parts.json");
            string carsJson = File.ReadAllText("../../../Datasets/cars.json");

            Console.WriteLine(ImportCars(db, carsJson));


            // 2. Export data
            //File.WriteAllText("../../../Datasets/products-in-range.json", GetProductsInRange(db));
            //File.WriteAllText("../../../Datasets/users-sold-products.json", GetSoldProducts(db));
            //File.WriteAllText("../../../Datasets/categories-by-products.json", GetCategoriesByProductsCount(db));
            //File.WriteAllText("../../../Datasets/users-and-products.json", GetUsersWithProducts(db));

            //string result = GetUsersWithProducts(db);
            //Console.WriteLine(result);
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)  // Task 0.9
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });
            IMapper mapper = new Mapper(config);

            var deserialize = JsonConvert.DeserializeObject<IEnumerable<SupplierDTO>>(inputJson);

            var suppliers = mapper.Map<IEnumerable<Supplier>>(deserialize);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count()}.";
        }

        public static string ImportParts(CarDealerContext context, string inputJson)  // Task 10
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });
            IMapper mapper = new Mapper(config);

            int[] supplierIds = context.Suppliers.Select(x => x.Id).ToArray();

            var deserialize = JsonConvert.DeserializeObject<IEnumerable<PartDTO>>(inputJson)
                .Where(x => supplierIds.Contains(x.SupplierId));

            var parts = mapper.Map<IEnumerable<Part>>(deserialize);

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count()}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)  // Task 11
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });
            IMapper mapper = new Mapper(config);

            var cars = new List<Car>();
            var deserialize = JsonConvert.DeserializeObject<IEnumerable<CarDTO>>(inputJson);

            foreach (var car in deserialize)
            {
                Car currentCar = mapper.Map<Car>(car);

                foreach (var partId in car.PartsId.Distinct())
                {
                    currentCar.PartCars.Add(new PartCar
                    {
                        PartId = partId
                    });
                }

                cars.Add(currentCar);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count()}.";
        }



    }
}