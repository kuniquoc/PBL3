using MySqlConnector;

namespace DaNangTourism.Server.Models.ScheduleModels
{
    public class ListSchedule
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
        public List<ScheduleElement> Items { get; set; }
        public ListSchedule()
        {
            Items = new List<ScheduleElement>();
        }
    }
    public class ScheduleElement
    {
        public int Id { get; set; }
        public ScheduleStatus Status { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string[] Destinations { get; set; }
        public DateOnly StartDate { get; set; }
        public int TotalDays { get; set; }
        public double TotalBudget { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ScheduleElement (MySqlDataReader reader)
        {
            Id = reader.GetInt32(reader.GetOrdinal("SheduleId"));
            string s = reader.GetString(reader.GetOrdinal("Status"));
            Status = (ScheduleStatus)Enum.Parse(typeof(ScheduleStatus), s); ;
            Title = reader.GetString(reader.GetOrdinal("Title"));
            Description = reader.GetString(reader.GetOrdinal("Description"));
            Destinations = reader.GetString(reader.GetOrdinal("Destinations")).Split(',');
            StartDate = reader.GetDateOnly(reader.GetOrdinal("StartDate"));
            TotalDays = reader.GetInt32(reader.GetOrdinal("TotalDays"));
            TotalBudget = reader.GetDouble(reader.GetOrdinal("TotalBudget"));
            UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt"));
        }

    }
}
