using MySqlConnector;
using System.Text.Json.Serialization;

namespace DaNangTourism.Server.Models.ScheduleModels
{
    public class UpdateScheduleModel
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = "";
        [JsonPropertyName("description")]
        public string Description { get; set; } = "";
        [JsonPropertyName("isPublic")]
        public bool IsPublic { get; set; }
        [JsonPropertyName("status")]
        public ScheduleStatus Status { get; set; }

        public UpdateScheduleModel() { }
        public UpdateScheduleModel(MySqlDataReader reader)
        {
            Title = reader.GetString(reader.GetOrdinal("Title"));
            Description = reader.GetString(reader.GetOrdinal("Description"));
            IsPublic = reader.GetBoolean(reader.GetOrdinal("IsPublic"));
            Status = Enum.Parse<ScheduleStatus>(reader.GetString(reader.GetOrdinal("Status")));
        }
    }
}
