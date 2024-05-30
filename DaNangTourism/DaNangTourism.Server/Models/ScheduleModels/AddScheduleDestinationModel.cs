using DaNangTourism.Server.ModelBindingConverter;
using MySqlConnector;
using System.IO.Pipelines;
using System.Text.Json.Serialization;

namespace DaNangTourism.Server.Models.ScheduleModels
{
    public class AddScheduleDestinationModel
    {
        [JsonPropertyName("scheduleId")]
        public int ScheduleId { get; set; } 
        [JsonPropertyName("destinationId")]
        public int DestinationId { get; set; }
        [JsonPropertyName("date")]
        public DateOnly Date { get; set; }
        [JsonPropertyName("arrivalTime")]
        [JsonConverter(typeof(TimeOnlyJsonConverter))]
        public TimeOnly ArrivalTime { get; set; }
        [JsonPropertyName("leaveTime")]
        [JsonConverter(typeof(TimeOnlyJsonConverter))]
        public TimeOnly LeaveTime { get; set; }
        [JsonPropertyName("budget")]
        public double Budget { get; set; }
        [JsonPropertyName("note")]
        public string? Note { get; set; }
        public AddScheduleDestinationModel() { }
        public AddScheduleDestinationModel(MySqlDataReader reader)
        {
            ScheduleId = reader.GetInt32(reader.GetOrdinal("ScheduleId"));
            DestinationId = reader.GetInt32(reader.GetOrdinal("DestinationId"));
            Date = reader.GetDateOnly(reader.GetOrdinal("Date"));
            ArrivalTime = reader.GetTimeOnly(reader.GetOrdinal("ArrivalTime"));
            LeaveTime = reader.GetTimeOnly(reader.GetOrdinal("LeaveTime"));
            Budget = reader.GetDouble(reader.GetOrdinal("Budget"));
            Note = reader.GetString(reader.GetOrdinal("Note"));
        }
    }
}
