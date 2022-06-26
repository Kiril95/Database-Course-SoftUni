namespace TeisterMask.DataProcessor.ExportDto
{
    public class EmployeesExportDTO
    {
        public string Username { get; set; }

        public TasksExportDTO[] Tasks { get; set; }
    }

    public class TasksExportDTO
    {
        public string TaskName { get; set; }

        public string OpenDate { get; set; }

        public string DueDate { get; set; }

        public string LabelType { get; set; }

        public string ExecutionType { get; set; }
    }
}
