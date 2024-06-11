using DaNangTourism.Server.ModelBindingConverter;
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
        [JsonConverter(typeof(EnumToStringJsonConverter<ScheduleStatus>))]
        public ScheduleStatus Status { get; set; } = ScheduleStatus.planning;

    }
}
