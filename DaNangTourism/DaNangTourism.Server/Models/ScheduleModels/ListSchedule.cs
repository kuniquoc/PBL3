using DaNangTourism.Server.ModelBindingConverter;
using MySqlConnector;
using System.Text.Json.Serialization;

namespace DaNangTourism.Server.Models.ScheduleModels
{
    public class ListSchedule
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }
        [JsonPropertyName("page")]
        public int Page { get; set; }
        [JsonPropertyName("limit")]
        public int Limit { get; set; }
        [JsonPropertyName("items")]
        public List<ScheduleElement> Items { get; set; }
        public ListSchedule()
        {
            Items = new List<ScheduleElement>();
        }
    }
    public class ScheduleElement
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("status")]
        [JsonConverter(typeof(EnumToStringJsonConverter<ScheduleStatus>))]
        public ScheduleStatus Status { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("destinations")]
        public string[] Destinations { get; set; }

        [JsonPropertyName("startDate")]
        public DateOnly? StartDate { get; set; }

        [JsonPropertyName("totalDays")]
        public int TotalDays { get; set; }

        [JsonPropertyName("totalBudget")]
        public double TotalBudget { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }
        public ScheduleElement(MySqlDataReader reader)
        {
            Id = reader.GetInt32(reader.GetOrdinal("ScheduleId"));
            string s = reader.GetString(reader.GetOrdinal("Status"));
            Status = (ScheduleStatus)Enum.Parse(typeof(ScheduleStatus), s); ;
            Title = reader.GetString(reader.GetOrdinal("Title"));
            Description = reader.GetString(reader.GetOrdinal("Description"));
            StartDate = reader.IsDBNull(reader.GetOrdinal("StartDate")) ? null : reader.GetDateOnly(reader.GetOrdinal("StartDate"));
            TotalDays = reader.GetInt32(reader.GetOrdinal("TotalDays"));
            TotalBudget = reader.GetDouble(reader.GetOrdinal("TotalBudget"));
            UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt"));
            Destinations = Array.Empty<string>();
        }
    }
}
