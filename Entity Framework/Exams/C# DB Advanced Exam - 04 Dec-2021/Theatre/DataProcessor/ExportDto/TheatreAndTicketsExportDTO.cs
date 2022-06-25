namespace Theatre.DataProcessor.ExportDto
{
    public class TheatreExportDTO
    {
        public string Name { get; set; }

        public int Halls { get; set; }

        public decimal TotalIncome { get; set; }

        public TicketExportDTO[] Tickets { get; set; }
    }

    public class TicketExportDTO
    {
        public decimal Price { get; set; }

        public int RowNumber { get; set; }
    }
}
