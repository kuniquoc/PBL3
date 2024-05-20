namespace DaNangTourism.Server.Models
{
    public enum TypeProcessing
    {
        Blog = 0,
        Review = 1,
        Destination = 2,
    }
    public class ProcessingReport
    {
        public int Id { get; set; }
        public int AdminId { get; set; }
        public int ReportId { get; set; }
        public string? Content { get; set; }
        public DateTime ProcessingTime { get; set; }
        public TypeProcessing Type { get; set; }
        public ProcessingReport() { }
        public ProcessingReport(int id, int adminId, int reportId, string content, DateTime processingTime, TypeProcessing type)
        {
            Id = id;
            AdminId = adminId;
            ReportId = reportId;
            Content = content;
            ProcessingTime = processingTime;
            Type = type;
        }
    }
}
