using DaNangTourism.Server.ModelBindingConverter;
using MySqlConnector;
using System.Text.Json.Serialization;

namespace DaNangTourism.Server.Models.ScheduleModels
{
    public class ScheduleDetail
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("status")]
        [JsonConverter(typeof(EnumToStringJsonConverter<ScheduleStatus>))]
        public ScheduleStatus Status { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("startDate")]
        public DateOnly? StartDate { get; set; }

        [JsonPropertyName("totalDays")]
        public int TotalDays { get; set; }

        [JsonPropertyName("totalBudget")]
        public double TotalBudget { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("creator")]
        public string? Creator { get; set; }

        [JsonPropertyName("isPublic")]
        public bool IsPublic { get; set; }

        [JsonPropertyName("days")]
        public List<ScheduleDay>? Days { get; set; }
        public ScheduleDetail(MySqlDataReader reader)
        {
            Id = reader.GetInt32(reader.GetOrdinal("ScheduleId"));
            Status = Enum.Parse<ScheduleStatus> (reader.GetString(reader.GetOrdinal("Status")));
            Title = reader.GetString(reader.GetOrdinal("Title"));
            Description = reader.GetString(reader.GetOrdinal("Description"));
            StartDate = reader.IsDBNull(reader.GetOrdinal("StartDate")) ? null: reader.GetDateOnly(reader.GetOrdinal("StartDate"));
            TotalDays = reader.GetInt32(reader.GetOrdinal("TotalDays"));
            TotalBudget = reader.GetDouble(reader.GetOrdinal("TotalBudget"));
            UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt"));
            Creator = reader.GetString(reader.GetOrdinal("Creator"));
            IsPublic = reader.GetBoolean(reader.GetOrdinal("IsPublic"));
        }
    }

    public class ScheduleDay
    {
        [JsonPropertyName("date")]
        public DateOnly Date { get; set; }

        [JsonPropertyName("destinations")]
        public List<DestinationOfDay> Destinations { get; set; }
        public ScheduleDay(DateOnly date)
        {
            Date = date;
            Destinations = new List<DestinationOfDay>();
        }
    }
    public class DestinationOfDay
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("destinationId")]
        public int DestinationId { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("address")]
        public string? Address { get; set; }

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

        public DestinationOfDay(MySqlDataReader reader)
        {
            Id = reader.GetInt32(reader.GetOrdinal("ScheduleDestinationId"));
            DestinationId = reader.GetInt32(reader.GetOrdinal("DestinationId"));
            Name = reader.GetString(reader.GetOrdinal("Name"));
            Address = reader.GetString(reader.GetOrdinal("Address"));
            ArrivalTime = reader.GetTimeOnly(reader.GetOrdinal("ArrivalTime"));
            LeaveTime = reader.GetTimeOnly(reader.GetOrdinal("LeaveTime"));
            Budget = reader.GetDouble(reader.GetOrdinal("Budget"));
            Note = reader.GetString(reader.GetOrdinal("Note"));
        }
    }
}
