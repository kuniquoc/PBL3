using MySqlConnector;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace DaNangTourism.Server.Models.DestinationModels
{
    public class ListDestination
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
        [JsonPropertyName("cost")]
        public double Cost { get; set; }
        [JsonPropertyName("openTime")]
        public TimeOnly OpenTime { get; set; }
        [JsonPropertyName("closeTime")]
        public TimeOnly CloseTime { get; set; }
        [JsonPropertyName("tags")]
        public string[] Tags { get; set; }
        [JsonPropertyName("favorite")]
        public bool Favourite { get; set; }

        public ListDestination(MySqlDataReader reader)
        {
            Id = reader.GetInt32(reader.GetOrdinal("DestinationId"));
            Name = reader.GetString(reader.GetOrdinal("Name"));
            Address = reader.GetString(reader.GetOrdinal("Address"));
            Image = reader.GetString(reader.GetOrdinal("Images")).Split(';').First();
            Rating = reader.GetFloat(reader.GetOrdinal("Rating"));
            Cost = reader.GetDouble(reader.GetOrdinal("Cost"));
            OpenTime = reader.GetTimeOnly(reader.GetOrdinal("OpenTime"));
            CloseTime = reader.GetTimeOnly(reader.GetOrdinal("CloseTime"));
            Tags = reader.GetString(reader.GetOrdinal("Tags")).Split(';');
            Favourite = reader.GetBoolean(reader.GetOrdinal("Favorite"));
        }
    }
}
