using DaNangTourism.Server.ModelBindingConverter;
using System.Text.Json.Serialization;

namespace DaNangTourism.Server.Models.ScheduleModels
{
  public class UpdateScheduleDestinationModel
  {
    [JsonPropertyName("scheduleId")]
    public int ScheduleId { get; set; }
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
  }
}
