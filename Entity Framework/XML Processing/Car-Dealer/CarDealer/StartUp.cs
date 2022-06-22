using AutoMapper;
using CarDealer.Data;
using CarDealer.Dtos.Import;
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
            string carsXml = File.ReadAllText("../../../Datasets/cars.xml");

            Console.WriteLine(ImportCars(db, carsXml));


            // 2. Export data
            //File.WriteAllText("../../../Datasets/products-in-range.xml", GetProductsInRange(db));

            //string result = GetUsersWithProducts(db);
            //Console.WriteLine(result);
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

            var parts = mapper.Map<IEnumerable<Part>>(deserialize);

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count()}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml) // Task 11
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });
            IMapper mapper = new Mapper(config);

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


    }
}