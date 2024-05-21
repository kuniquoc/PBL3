using MySqlConnector;
using System.Text.Json.Serialization;

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
        public TimeOnly OpenTime { get; set; }
        [JsonPropertyName("closeTime")]
        public TimeOnly CloseTime { get; set; }
        [JsonPropertyName("tags")]
        public string[] Tags { get; set; }
        [JsonPropertyName("favorite")]
        public bool Favourite { get; set; }

        public ListDestination(MySqlDataReader reader)
        {
            Id = reader.GetInt32("DestinationId");
            Name = reader.GetString("Name");
            Address = reader.GetString("Address");
            Image = reader.GetString("Images").Split(';').First();
            Rating = reader.GetFloat("Rating");
            Cost = reader.GetDouble("Cost");
            OpenTime = reader.GetTimeOnly("OpenTime");
            CloseTime = reader.GetTimeOnly("CloseTime");
            Tags = reader.GetString("Tags").Split(';');
            // giá trị mặc định là không phải yêu thích
            Favourite = false;
        }
    }
}
