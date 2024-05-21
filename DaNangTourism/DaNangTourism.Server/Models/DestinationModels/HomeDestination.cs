using MySqlConnector;
using System.Text.Json.Serialization;

namespace DaNangTourism.Server.Models
{
    public class HomeDestination
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("address")]
        public string Address { get; set; }
        [JsonPropertyName("image")]
        public string Image { get; set; }
        [JsonPropertyName("rating")]
        public double Rating { get; set; }
        public HomeDestination(MySqlDataReader reader)
        {
            Id = reader.GetInt32("DestinationId");
            Name = reader.GetString("Name");
            Address = reader.GetString("Address");
            Image = reader.GetString("Images").Split(';').FirstOrDefault();
            Rating = reader.GetFloat("Rating");
        }
    }
}
