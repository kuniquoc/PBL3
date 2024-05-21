using MySqlConnector;
using System.Text.Json.Serialization;

namespace DaNangTourism.Server.Models.DestinationModels
{
    public class DestinationModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("localName")]
        public string LocalName { get; set; }
        [JsonPropertyName("address")]
        public string Address { get; set; }
        [JsonPropertyName("googleMapUrl")]
        public string GoogleMapUrl { get; set; }
        [JsonPropertyName("cost")]
        public double Cost { get; set; }
        [JsonPropertyName("openTime")]
        public TimeOnly OpenTime { get; set; }
        [JsonPropertyName("closeTime")]
        public TimeOnly CloseTime { get; set; }
        [JsonPropertyName("images")]
        public string[] Images { get; set; }
        [JsonPropertyName("tags")]
        public string[] Tags { get; set; }
        [JsonPropertyName("introduction")]
        public string Introduction { get; set; }

        public DestinationModel(MySqlDataReader reader)
        {
            Name = reader.GetString("Name");
            LocalName = reader.GetString("LocalName");
            Address = reader.GetString("Address");
            GoogleMapUrl = reader.GetString("GoogleMapUrl");
            Cost = reader.GetDouble("Cost");
            OpenTime = reader.GetTimeOnly("OpenTime");
            CloseTime = reader.GetTimeOnly("CloseTime");
            Images = reader.GetString("Images").Split(';');
            Tags = reader.GetString("Tags").Split(';');
            Introduction = reader.GetString("Introduction");
        }
    }
}
