using AutoMapper;
using CarDealer.Data;
using CarDealer.Dtos.Import;
using CarDealer.Dtos.Export;
using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var db = new CarDealerContext();
            //db.Database.EnsureCreated();

            // 1. Import data
            //string suppliersXml = File.ReadAllText("../../../Datasets/suppliers.xml");
            //string partsXml = File.ReadAllText("../../../Datasets/parts.xml");
            //string carsXml = File.ReadAllText("../../../Datasets/cars.xml");
            //string customersXml = File.ReadAllText("../../../Datasets/customers.xml");
            //string salesXml = File.ReadAllText("../../../Datasets/sales.xml");

            //Console.WriteLine(ImportSales(db, salesXml));


            // 2. Export data
            //File.WriteAllText("../../../Datasets/cars.xml", GetCarsWithDistance(db));
            //File.WriteAllText("../../../Datasets/bmw-cars.xml", GetCarsFromMakeBmw(db));
            //File.WriteAllText("../../../Datasets/local-suppliers.xml", GetLocalSuppliers(db));
            //File.WriteAllText("../../../Datasets/cars-and-parts.xml", GetCarsWithTheirListOfParts(db));
            //File.WriteAllText("../../../Datasets/customers-total-sales.xml", GetTotalSalesByCustomer(db));
            File.WriteAllText("../../../Datasets/sales-discounts.xml", GetSalesWithAppliedDiscount(db));

            string result = GetSalesWithAppliedDiscount(db);
            Console.WriteLine(result);
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml) // Task 0.9
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });
            IMapper mapper = new Mapper(config);

            XmlSerializer serializer = new XmlSerializer(typeof(SupplierDTO[]), new XmlRootAttribute("Suppliers"));
            StringReader reader = new StringReader(inputXml);

            var deserialize = (SupplierDTO[])serializer.Deserialize(reader);

            var suppliers = mapper.Map<IEnumerable<Supplier>>(deserialize);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count()}";
        }

        public static string ImportParts(CarDealerContext context, string inputXml) // Task 10
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });
            IMapper mapper = new Mapper(config);

            XmlSerializer serializer = new XmlSerializer(typeof(PartDTO[]), new XmlRootAttribute("Parts"));
            StringReader reader = new StringReader(inputXml);

            var suppliesIds = context.Suppliers.Select(x => x.Id);
            var deserialize = (PartDTO[])serializer.Deserialize(reader);
            var filtered = deserialize.Where(p => suppliesIds.Contains(p.SupplierId));

            var parts = mapper.Map<IEnumerable<Part>>(filtered);

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count()}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml) // Task 11
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CarDTO[]), new XmlRootAttribute("Parts"));
            StringReader reader = new StringReader(inputXml);

            var cars = new List<Car>();
            var parts = context.Parts.Select(x => x.Id).ToList();
            var deserialize = (CarDTO[])serializer.Deserialize(reader);

            foreach (var current in deserialize)
            {
                var distinctParts = current.CarParts.Select(x => x.Id).Distinct();
                var allParts = distinctParts.Intersect(parts);

                var car = new Car
                {
                    Make = current.Make,
                    Model = current.Model,
                    TravelledDistance = current.TraveledDistance,
                };

                foreach (var part in parts)
                {
                    var partCar = new PartCar
                    {
                        PartId = part
                    };

                    car.PartCars.Add(partCar);
                }

                cars.Add(car);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count()}";
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml) // Task 12
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });
            IMapper mapper = new Mapper(config);

            XmlSerializer serializer = new XmlSerializer(typeof(CustomerDTO[]), new XmlRootAttribute("Customers"));
            StringReader reader = new StringReader(inputXml);

            var deserialize = (CustomerDTO[])serializer.Deserialize(reader);

            var customers = mapper.Map<IEnumerable<Customer>>(deserialize);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count()}";
        }

        public static string ImportSales(CarDealerContext context, string inputXml) // Task 13
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });
            IMapper mapper = new Mapper(config);

            XmlSerializer serializer = new XmlSerializer(typeof(SaleDTO[]), new XmlRootAttribute("Sales"));
            StringReader reader = new StringReader(inputXml);

            var carIds = context.Cars.Select(x => x.Id);
            var deserialize = (SaleDTO[])serializer.Deserialize(reader);
            var filtered = deserialize.Where(x => carIds.Contains(x.CarId));

            var sales = mapper.Map<IEnumerable<Sale>>(filtered);

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count()}";
        }

        public static string GetCarsWithDistance(CarDealerContext context)  // Task 14
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(CarWithDistanceDTO[]), new XmlRootAttribute("cars"));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            StringWriter writer = new StringWriter(sb);

            var cars = context.Cars
                .Where(x => x.TravelledDistance > 2000000)
                .Select(p => new CarWithDistanceDTO
                {
                    Make = p.Make,
                    Model = p.Model,
                    TravelledDistance = p.TravelledDistance,
                })
                .OrderBy(x => x.Make)
                .ThenBy(x => x.Model)
                .Take(10)
                .ToArray();

            serializer.Serialize(writer, cars, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)  // Task 15
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(BmwCarsDTO[]), new XmlRootAttribute("cars"));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            StringWriter writer = new StringWriter(sb);

            var youAreWalkingWhenItsWinterCars = context.Cars
                .Where(x => x.Make == "BMW")
                .Select(p => new BmwCarsDTO
                {
                    Id = p.Id,
                    Model = p.Model,
                    TravelledDistance = p.TravelledDistance,
                })
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .ToArray();

            serializer.Serialize(writer, youAreWalkingWhenItsWinterCars, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetLocalSuppliers(CarDealerContext context)  // Task 16
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(LocalSuppliersDTO[]), new XmlRootAttribute("suppliers"));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            StringWriter writer = new StringWriter(sb);

            var suppliers = context.Suppliers
                .Where(x => !x.IsImporter)
                .Select(p => new LocalSuppliersDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Parts = p.Parts.Count()
                })
                .ToArray();

            serializer.Serialize(writer, suppliers, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)  // Task 17
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(CarExportDTO[]), new XmlRootAttribute("cars"));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            StringWriter writer = new StringWriter(sb);

            var carsWithParts = context.Cars
                .Select(x => new CarExportDTO
                {
                    Make = x.Make,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance,
                    CarParts = x.PartCars.Select(p => new PartsDTO
                    {
                        Name = p.Part.Name,
                        Price = p.Part.Price
                    })
                    .OrderByDescending(x => x.Price)
                    .ToArray()
                })
                .OrderByDescending(x => x.TravelledDistance)
                .ThenBy(x => x.Model)
                .Take(5)
                .ToArray();

            serializer.Serialize(writer, carsWithParts, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)  // Task 18
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(TotalSalesDTO[]), new XmlRootAttribute("customers"));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            StringWriter writer = new StringWriter(sb);

            var customers = context.Sales
                .Where(x => x.Customer.Sales.Count() > 0)
                .Select(x => new TotalSalesDTO
                {
                    Name = x.Customer.Name,
                    BoughtCars = x.Customer.Sales.Count(),
                    MoneySpent = x.Car.PartCars.Sum(p => p.Part.Price)
                })
                .OrderByDescending(x => x.MoneySpent)
                .ToArray();

            serializer.Serialize(writer, customers, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)  // Task 19
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(SalesWithDiscountDTO[]), new XmlRootAttribute("sales"));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            StringWriter writer = new StringWriter(sb);

            var customers = context.Sales
                .Select(x => new SalesWithDiscountDTO
                {
                    Car = new CarSaleDTO
                    {
                        Make = x.Car.Make,
                        Model = x.Car.Model,
                        TravelledDistance = x.Car.TravelledDistance
                    },
                    Discount = x.Discount,
                    CustomerName = x.Customer.Name,
                    Price = x.Car.PartCars.Sum(p => p.Part.Price),
                    PriceWithtDiscount = x.Car.PartCars.Sum(p => p.Part.Price) - x.Car.PartCars.Sum(p => p.Part.Price) * x.Discount / 100
                })
                .ToArray();

            serializer.Serialize(writer, customers, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}