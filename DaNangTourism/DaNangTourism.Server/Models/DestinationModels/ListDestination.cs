using MySqlConnector;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace DaNangTourism.Server.Models
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
        [JsonPropertyName("cost")]
        public double Cost { get; set; }
        [JsonPropertyName("rating")]
        public double Rating { get; set; }
        [JsonPropertyName("openTime")]
        public TimeSpan OpenTime { get; set; }
        [JsonPropertyName("closeTime")]
        public TimeSpan CloseTime { get; set; }
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
            OpenTime = reader.GetTimeSpan(reader.GetOrdinal("OpenTime"));
            CloseTime = reader.GetTimeSpan(reader.GetOrdinal("CloseTime"));
            Tags = reader.GetString(reader.GetOrdinal("Tags")).Split(';');
            // giá trị mặc định là không phải yêu thích
            Favourite = false;
        }
    }
}
