
namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportShells(ArtilleryContext context, double shellWeight)
        {
            var shells = context.Shells
                .ToArray()
                .Where(x => x.ShellWeight > shellWeight)
                .Select(x => new ShellExportDTO
                {
                    ShellWeight = x.ShellWeight,
                    Caliber = x.Caliber,
                    Guns = x.Guns
                        .ToArray()
                        .Where(g => g.GunType == GunType.AntiAircraftGun)
                        .Select(g => new GunExportDto
                        {
                            GunType = g.GunType.ToString(),
                            GunWeight = g.GunWeight,
                            BarrelLength = g.BarrelLength,
                            Range = g.Range > 3000 ? "Long-range" : "Regular range"
                        })
                        .OrderByDescending(g => g.GunWeight)
                        .ToArray()
                })
                .OrderBy(s => s.ShellWeight)
                .ToArray();

            return JsonConvert.SerializeObject(shells, Formatting.Indented);
        }

        public static string ExportGuns(ArtilleryContext context, string manufacturer)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(GunCountryExportDTO[]), new XmlRootAttribute("Guns"));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            using StringWriter writer = new StringWriter(sb);

            var guns = context.Guns
                .Where(x => x.Manufacturer.ManufacturerName == manufacturer)
                .Select(g => new GunCountryExportDTO
                {
                    Manufacturer = g.Manufacturer.ManufacturerName,
                    GunType = g.GunType.ToString(),
                    GunWeight = g.GunWeight,
                    Barrellength = g.BarrelLength,
                    Range = g.Range,
                    Countries = g.CountriesGuns
                        .Where(g => g.Country.ArmySize > 4500000)
                        .Select(cg => new CountryExportDTO
                        {
                            Country = cg.Country.CountryName,
                            ArmySize = cg.Country.ArmySize
                        })
                        .OrderBy(x => x.ArmySize)
                        .ToArray()
                })
                .OrderBy(x => x.Barrellength)
                .ToArray();

            serializer.Serialize(writer, guns, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}
