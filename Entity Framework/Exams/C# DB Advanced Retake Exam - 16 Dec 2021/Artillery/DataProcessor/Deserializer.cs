namespace Artillery.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage =
                "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            List<Country> countries = new List<Country>();

            XmlSerializer serializer = new XmlSerializer(typeof(CountryImportDTO[]), new XmlRootAttribute("Countries"));
            using StringReader reader = new StringReader(xmlString);

            var deserialize = (CountryImportDTO[])serializer.Deserialize(reader);

            foreach (var countryItem in deserialize)
            {
                if (!IsValid(countryItem))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Country country = new Country()
                {
                    CountryName = countryItem.CountryName,
                    ArmySize = countryItem.ArmySize
                };

                countries.Add(country);
                sb.AppendLine(string.Format(SuccessfulImportCountry, country.CountryName, country.ArmySize));
            }

            context.Countries.AddRange(countries);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            List<Manufacturer> manufacturers = new List<Manufacturer>();

            XmlSerializer serializer = new XmlSerializer(typeof(ManufacturerImportDTO[]), new XmlRootAttribute("Manufacturers"));
            using StringReader reader = new StringReader(xmlString);

            var deserialize = (ManufacturerImportDTO[])serializer.Deserialize(reader);

            foreach (var manufacturer in deserialize)
            {
                if (!IsValid(manufacturer))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var name = manufacturers.FirstOrDefault(x => x.ManufacturerName == manufacturer.ManufacturerName);
                if (name != null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                string[] split = manufacturer.Founded.Split(", ");
                Stack<string> founded = new Stack<string>(split);
                string country = founded.Pop();
                string town = founded.Pop();
                string townCountry = $"{town}, {country}";

                Manufacturer manu = new Manufacturer()
                {
                    ManufacturerName = manufacturer.ManufacturerName,
                    Founded = manufacturer.Founded
                };

                manufacturers.Add(manu);
                sb.AppendLine(string.Format(SuccessfulImportManufacturer, manu.ManufacturerName, townCountry));
            }

            context.Manufacturers.AddRange(manufacturers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            List<Shell> shells = new List<Shell>();

            XmlSerializer serializer = new XmlSerializer(typeof(ShellImportDTO[]), new XmlRootAttribute("Shells"));
            using StringReader reader = new StringReader(xmlString);

            var deserialize = (ShellImportDTO[])serializer.Deserialize(reader);

            foreach (var shellItem in deserialize)
            {
                if (!IsValid(shellItem))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Shell shell = new Shell()
                {
                    ShellWeight = shellItem.ShellWeight,
                    Caliber = shellItem.Caliber
                };

                shells.Add(shell);
                sb.AppendLine(string.Format(SuccessfulImportShell, shell.Caliber, shell.ShellWeight));
            }

            context.Shells.AddRange(shells);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            List<Gun> guns = new List<Gun>();

            var deserialize = JsonConvert.DeserializeObject<IEnumerable<GunImportDTO>>(jsonString);

            foreach (var gunItem in deserialize)
            {
                if (!IsValid(gunItem))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                GunType gunType;
                bool check = Enum.TryParse(gunItem.GunType, out gunType);
                if (!check)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Gun gun = new Gun()
                {
                    ManufacturerId = gunItem.ManufacturerId,
                    GunWeight = gunItem.GunWeight,
                    BarrelLength = gunItem.BarrelLength,
                    NumberBuild = gunItem.NumberBuild,
                    Range = gunItem.Range,
                    GunType = gunType,
                    ShellId = gunItem.ShellId
                };

                if (gunItem.Countries.Any())
                {
                    foreach (var country in gunItem.Countries)
                    {
                        var currentCountry = context.Countries.FirstOrDefault(c => c.Id == country.Id);

                        gun.CountriesGuns.Add(new CountryGun
                        {
                            Country = currentCountry,
                            Gun = gun
                        });
                    }
                }

                guns.Add(gun);
                sb.AppendLine(string.Format(SuccessfulImportGun, gun.GunType, gun.GunWeight, gun.BarrelLength));
            }

            context.Guns.AddRange(guns);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
