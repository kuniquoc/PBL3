using DaNangTourism.Server.ModelBindingConverter;
using MySqlConnector;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DaNangTourism.Server.Models.DestinationModels
{
    public class InputDestinationModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = "";
        [JsonPropertyName("localName")]
        public string LocalName { get; set; } = "";
        [JsonPropertyName("address")]
        public string Address { get; set; } = "";
        [JsonPropertyName("googleMapUrl")]
        public string GoogleMapUrl { get; set; } = "";
        [JsonPropertyName("cost")]
        public double Cost { get; set; }
        [JsonPropertyName("openTime")]
        [JsonConverter(typeof(TimeOnlyJsonConverter))]
        public TimeOnly OpenTime { get; set; }
        [JsonPropertyName("closeTime")]
        [JsonConverter(typeof(TimeOnlyJsonConverter))]
        public TimeOnly CloseTime { get; set; }
        [JsonPropertyName("images")]
        public string[] Images { get; set; } = [];
        [JsonPropertyName("tags")]
        public string[] Tags { get; set; } = [];
        [JsonPropertyName("introduction")]
        public string Introduction { get; set; } = "";

        public InputDestinationModel() { }
        public InputDestinationModel(MySqlDataReader reader)
        {
            Name = reader.GetString(reader.GetOrdinal("Name"));
            LocalName = reader.GetString(reader.GetOrdinal("LocalName"));
            Address = reader.GetString(reader.GetOrdinal("Address"));
            GoogleMapUrl = reader.GetString(reader.GetOrdinal("GoogleMapUrl"));
            Cost = reader.GetDouble(reader.GetOrdinal("Cost"));
            OpenTime = reader.GetTimeOnly(reader.GetOrdinal("OpenTime"));
            CloseTime = reader.GetTimeOnly(reader.GetOrdinal("CloseTime"));
            Images = reader.GetString(reader.GetOrdinal("Images")).Split(';');
            Tags = reader.GetString(reader.GetOrdinal("Tags")).Split(';');
            Introduction = reader.GetString(reader.GetOrdinal("Introduction"));
        }
    }
}
