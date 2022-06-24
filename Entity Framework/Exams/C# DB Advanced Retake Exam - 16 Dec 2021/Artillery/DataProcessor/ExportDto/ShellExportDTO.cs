namespace Artillery.DataProcessor.ExportDto
{
    public class ShellExportDTO
    {
        public double ShellWeight { get; set; }

        public string Caliber { get; set; }

        public GunExportDto[] Guns { get; set; }
    }

    public class GunExportDto
    {
        public string GunType { get; set; }

        public int GunWeight { get; set; }

        public double BarrelLength { get; set; }

        public string Range { get; set; }
    }
}
