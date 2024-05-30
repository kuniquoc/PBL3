using MySqlConnector;
using System.Text.Json.Serialization;

namespace DaNangTourism.Server.Models.ScheduleModels
{
    public class PublicSchedule
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }
        [JsonPropertyName("page")]
        public int Page { get; set; }
        [JsonPropertyName("limit")]
        public int Limit { get; set; }
        [JsonPropertyName("items")]
        public List<PublicScheduleElement> Items { get; set; }
        public PublicSchedule()
        {
            Items = new List<PublicScheduleElement>();
        }
    }
    public class PublicScheduleElement
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("destinations")]
        public string[] Destinations { get; set; }

        [JsonPropertyName("totalDays")]
        public int TotalDays { get; set; }

        [JsonPropertyName("totalBudget")]
        public double TotalBudget { get; set; }

        [JsonPropertyName("creator")]
        public string Creator { get; set; }
        public PublicScheduleElement(MySqlDataReader reader)
        {
            Id = reader.GetInt32(reader.GetOrdinal("ScheduleId"));
            Title = reader.GetString(reader.GetOrdinal("Title"));
            Description = reader.GetString(reader.GetOrdinal("Description"));
            TotalDays = reader.GetInt32(reader.GetOrdinal("TotalDays"));
            TotalBudget = reader.GetDouble(reader.GetOrdinal("TotalBudget"));
            Creator = reader.GetString(reader.GetOrdinal("Creator"));
            Destinations = Array.Empty<string>();
        }

    }
}
