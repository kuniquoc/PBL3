using MySqlConnector;
using System.Text.Json.Serialization;

namespace DaNangTourism.Server.Models.DestinationModels
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
        public float Rating { get; set; }
        public HomeDestination(MySqlDataReader reader)
        {
            Id = reader.GetInt32(reader.GetOrdinal("DestinationId")); ;
            Name = reader.GetString(reader.GetOrdinal("Name"));
            Address = reader.GetString(reader.GetOrdinal("Address"));
            Image = reader.GetString(reader.GetOrdinal("Images")).Split(';').First();
            Rating = reader.GetFloat(reader.GetOrdinal("Rating"));
        }
    }
}
