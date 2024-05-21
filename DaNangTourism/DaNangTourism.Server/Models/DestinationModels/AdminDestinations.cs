using DaNangTourism.Server.Helper;
using MySqlConnector;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DaNangTourism.Server.Models
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
        [JsonPropertyName("created_at")]
        [JsonConverter(typeof(ConvertToISO8061DateTime))]
        public DateTime Created_At { get; set; } 
        public DestinationElement () { }
        public DestinationElement(MySqlDataReader reader)
        {
            Id = reader.GetInt32("d.DestinationId");
            Name = reader.GetString("d.Name");
            Address = reader.GetString("d.Address");
            Rating = reader.GetFloat("d.Rating");
            CountOfReview = reader.GetInt32("Review");
            CountOfFavorite = reader.GetInt32("Favorite");
            Created_At = reader.GetDateTime("d.Created_At");
        }
    }
}
