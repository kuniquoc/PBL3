using MySqlConnector;
using System.IO.Pipelines;
using System.Text.Json.Serialization;

namespace DaNangTourism.Server.Models.ScheduleModels
{
    public class ScheduleDestination
    {
        [JsonPropertyName("scheduleId")]
        public int? ScheduleId { get; set; }
        [JsonPropertyName("destinationId")]
        public int? DestinationId { get; set; }
        [JsonPropertyName("date")]
        public DateOnly Date { get; set; }
        [JsonPropertyName("arrivalTime")]
        public TimeOnly ArrivalTime { get; set; }
        [JsonPropertyName("leaveTime")]
        public TimeOnly LeaveTime { get; set; }
        [JsonPropertyName("budget")]
        public double Budget { get; set; }
        [JsonPropertyName("note")]
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
