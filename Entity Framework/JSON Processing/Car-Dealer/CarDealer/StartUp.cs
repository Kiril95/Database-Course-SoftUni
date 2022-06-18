using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
            //string carsJson = File.ReadAllText("../../../Datasets/cars.json");
            //string customersJson = File.ReadAllText("../../../Datasets/customers.json");
            //string salesJson = File.ReadAllText("../../../Datasets/sales.json");

            //Console.WriteLine(ImportSales(db, salesJson));


            // 2. Export data
            //File.WriteAllText("../../../Datasets/ordered-customers.json", GetOrderedCustomers(db));
            //File.WriteAllText("../../../Datasets/toyota-cars.json", GetCarsFromMakeToyota(db));
            //File.WriteAllText("../../../Datasets/local-suppliers.json", GetLocalSuppliers(db));
            //File.WriteAllText("../../../Datasets/cars-and-parts.json", GetCarsWithTheirListOfParts(db));
            //File.WriteAllText("../../../Datasets/customers-total-sales.json", GetTotalSalesByCustomer(db));
            File.WriteAllText("../../../Datasets/sales-discounts.json", GetSalesWithAppliedDiscount(db));

            string result = GetSalesWithAppliedDiscount(db);
            Console.WriteLine(result);
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

        public static string ImportCustomers(CarDealerContext context, string inputJson)  // Task 12
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });
            IMapper mapper = new Mapper(config);

            var deserialize = JsonConvert.DeserializeObject<IEnumerable<CustomerDTO>>(inputJson);

            var customers = mapper.Map<IEnumerable<Customer>>(deserialize);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count()}.";
        }

        public static string ImportSales(CarDealerContext context, string inputJson)  // Task 13
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });
            IMapper mapper = new Mapper(config);

            var deserialize = JsonConvert.DeserializeObject<IEnumerable<SaleDTO>>(inputJson);

            var sales = mapper.Map<IEnumerable<Sale>>(deserialize);

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count()}.";
        }

        public static string GetOrderedCustomers(CarDealerContext context)  // Task 14
        {
            var customers = context.Customers
                .Select(x => new
                {
                    Name = x.Name,
                    BirthDate = x.BirthDate,
                    IsYoungDriver = x.IsYoungDriver
                })
                .OrderBy(x => x.BirthDate)
                .ThenBy(x => x.IsYoungDriver)
                .ToArray();

            var settings = new JsonSerializerSettings()
            {
                DateFormatString = "dd/MM/yyyy",
                Formatting = Formatting.Indented,
            };

            return JsonConvert.SerializeObject(customers, settings);
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)  // Task 15
        {
            var cars = context.Cars
                .Where(x => x.Make == "Toyota")
                .Select(x => new
                {
                    Id = x.Id,
                    Make = x.Make,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance
                })
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .ToArray();

            return JsonConvert.SerializeObject(cars, Formatting.Indented);
        }

        public static string GetLocalSuppliers(CarDealerContext context)  // Task 16
        {
            var suppliers = context.Suppliers
                .Where(x => !x.IsImporter)
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                    PartsCount = x.Parts.Count()
                })
                .ToArray();

            return JsonConvert.SerializeObject(suppliers, Formatting.Indented);
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)  // Task 17
        {
            var carsAndParts = context.Cars
                 .Select(x => new
                 {
                     car = new
                     {
                         Make = x.Make,
                         Model = x.Model,
                         TravelledDistance = x.TravelledDistance,
                     },
                     parts = x.PartCars.Select(p => new
                     {
                         Name = p.Part.Name,
                         Price = p.Part.Price.ToString("f2")
                     })
                 })
                .ToArray();

            return JsonConvert.SerializeObject(carsAndParts, Formatting.Indented);
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)  // Task 18
        {
            var sales = context.Sales
                .Where(x => x.Customer.Sales.Any())
                .Select(x => new
                {
                    FullName = x.Customer.Name,
                    BoughtCars = x.Customer.Sales.Count(),
                    SpentMoney = x.Customer.Sales.Sum(y => y.Car.PartCars.Sum(p => p.Part.Price))
                })
                .OrderByDescending(x => x.SpentMoney)
                .ThenByDescending(x => x.BoughtCars)
                .Distinct()
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

            return JsonConvert.SerializeObject(sales, settings);
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)  // Task 19
        {
            var sales = context.Sales
                .Take(10)
                .Select(x => new
                {
                    car = new
                    {
                        Make = x.Car.Make,
                        Model = x.Car.Model,
                        TravelledDistance = x.Car.TravelledDistance
                    },
                    customerName = x.Customer.Name,
                    Discount = x.Discount.ToString("f2"),
                    price = x.Car.PartCars.Sum(p => p.Part.Price).ToString("f2"),
                    priceWithDiscount = (x.Car.PartCars.Sum(p => p.Part.Price) - x.Car.PartCars.Sum(p => p.Part.Price) * x.Discount / 100).ToString("f2")
                })
                .ToArray();

            return JsonConvert.SerializeObject(sales, Formatting.Indented);
        }
    }
}