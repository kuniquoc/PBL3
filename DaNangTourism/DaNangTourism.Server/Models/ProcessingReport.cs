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
        private int _id;
        private int _adminId;
        private int _reportId;
        private string? _content;
        private DateTime _processingTime;
        private TypeProcessing _type;
        public int Id { get { return _id; } set { _id = value; } }
        public int AdminId { get { return _adminId; } set { _adminId = value; } }
        public int ReportId { get { return _reportId; } set { _reportId = value; } }
        public string? Content { get { return _content; } set { _content = value; } }
        public DateTime ProcessingTime { get { return _processingTime; } set { _processingTime = value; } }
        public TypeProcessing Type { get { return _type; } set { _type = value; } }
        public ProcessingReport() { }
        public ProcessingReport(int id, int adminId, int reportId, string content, DateTime processingTime, TypeProcessing type)
        {
            _id = id;
            _adminId = adminId;
            _reportId = reportId;
            _content = content;
            _processingTime = processingTime;
            _type = type;
        }
    }
}
