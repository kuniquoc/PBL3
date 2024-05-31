using DaNangTourism.Server.ModelBindingConverter;
using MySqlConnector;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DaNangTourism.Server.Models.DestinationModels
{
  public class AdminDestinations
  {
    [JsonPropertyName("total")]
    public int Total { get; set; }
    [JsonPropertyName("page")]
    public int Page { get; set; }
    [JsonPropertyName("limit")]
    public int Limit { get; set; }
    [JsonPropertyName("items")]
    public List<DestinationElement> Items { get; set; }
    public AdminDestinations()
    {
      Items = new List<DestinationElement>();
    }
  }
  public class DestinationElement
  {
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("address")]
    public string Address { get; set; }
    [JsonPropertyName("rating")]
    public float Rating { get; set; }
    [JsonPropertyName("review")]
    public int CountOfReview { get; set; }
    [JsonPropertyName("favorite")]
    public int CountOfFavorite { get; set; }
    [JsonPropertyName("createdAt")]
    [JsonConverter(typeof(ConvertToISO8061DateTime))]
    public DateTime Created_At { get; set; }
    public DestinationElement()
    {
      Name = string.Empty;
      Address = string.Empty;
    }
    public DestinationElement(MySqlDataReader reader)
    {
      Id = reader.GetInt32(reader.GetOrdinal("DestinationId"));
      Name = reader.GetString(reader.GetOrdinal("Name"));
      Address = reader.GetString(reader.GetOrdinal("Address"));
      Rating = reader.GetFloat(reader.GetOrdinal("Rating"));
      CountOfReview = reader.GetInt32(reader.GetOrdinal("CountOfReview"));
      CountOfFavorite = reader.GetInt32(reader.GetOrdinal("CountOfFavorite"));
      Created_At = reader.GetDateTime(reader.GetOrdinal("Created_At"));
    }
  }
}
