using MySqlConnector;
using System.IO.Pipelines;

namespace DaNangTourism.Server.Models.ScheduleModels
{
    public class ScheduleDestination
    {
        public int ScheduleId { get; set; }
        public int DestinationId { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly ArrivalTime { get; set; }
        public TimeOnly LeaveTime { get; set; }
        public double Budget { get; set; }
        public string? Note { get; set; }
        public ScheduleDestination() { }
        public ScheduleDestination(MySqlDataReader reader)
        {
            ScheduleId = reader.GetInt32(reader.GetOrdinal("ScheduleId"));
            DestinationId = reader.GetInt32(reader.GetOrdinal("DestinationId"));
            Date = reader.GetDateOnly(reader.GetOrdinal("Date"));
            ArrivalTime = reader.GetTimeOnly(reader.GetOrdinal("ArrivalTime"));
            LeaveTime = reader.GetTimeOnly(reader.GetOrdinal("LeaveTime"));
            Budget = reader.GetDouble(reader.GetOrdinal("LeaveTime"));
            Note = reader.GetString(reader.GetOrdinal("Note"));
        }
    }
}
