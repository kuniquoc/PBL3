using System.Text.Json.Serialization;

namespace DaNangTourism.Server.Models.ScheduleModels
{
    public class InputSchedule
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } 
        [JsonPropertyName("description")]
        public string Description { get; set; } 
        [JsonPropertyName("isPublic")]
        public bool IsPublic { get; set; }
    }
}
