using MySqlConnector;

namespace DaNangTourism.Server.Models
{
    public enum ScheduleStatus
    {
        planning = 0,
        processing = 1,
        completed = 2,
        cancelled = 3,
    }
    public class Schedule
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Describe { get; set; }
        public ScheduleStatus Status { get; set; }
        public bool IsPublic { get; set; }
        public Schedule()
        {

        }
        public Schedule(MySqlDataReader reader)
        {
            Id = reader.GetInt32("schedule_id");
            UserId = reader.GetInt32("user_id");
            Name = reader.GetString("name");
            Describe = reader.GetString("describe");
            string s = reader.GetString(reader.GetOrdinal("status"));
            Status = (ScheduleStatus)Enum.Parse(typeof(ScheduleStatus), s); ;
            IsPublic = reader.GetBoolean("is_public");
        }
    }
}
