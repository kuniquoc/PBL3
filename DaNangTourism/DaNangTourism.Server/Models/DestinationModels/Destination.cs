using DaNangTourism.Server.ModelBindingConverter;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Hosting;
using MySqlConnector;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;

namespace DaNangTourism.Server.Models.DestinationModels
{
    public class Destination
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("localName")]
        public string LocalName { get; set; }
        [JsonPropertyName("address")]
        public string Address { get; set; }
        [JsonPropertyName("images")]
        public string[] Images { get; set; }
        [JsonPropertyName("cost")]
        public double Cost { get; set; }
        [JsonPropertyName("openTime")]
        public TimeOnly OpenTime { get; set; }
        [JsonPropertyName("closeTime")]
        public TimeOnly CloseTime { get; set; }
        [JsonPropertyName("tags")]
        public string[] Tags { get; set; }
        [JsonPropertyName("introduction")]
        public string Introduction { get; set; }
        [JsonPropertyName("googleMapUrl")]
        public string GoogleMapUrl { get; set; }
        [JsonPropertyName("rating")]
        public float Rating { get; set; }
        [JsonPropertyName("created_at")]
        [JsonConverter(typeof(ConvertToISO8061DateTime))]
        public DateTime Created_At { get; set; }

        public Destination(MySqlDataReader reader)
        {
            Id = reader.GetInt32(reader.GetOrdinal("DestinationId"));
            Name = reader.GetString(reader.GetOrdinal("Name"));
            LocalName = reader.GetString(reader.GetOrdinal("LocalName"));
            Address = reader.GetString(reader.GetOrdinal("Address"));
            Images = reader.GetString(reader.GetOrdinal("Images")).Split(';');
            Cost = reader.GetDouble(reader.GetOrdinal("Cost"));
            OpenTime = reader.GetTimeOnly(reader.GetOrdinal("OpenTime"));
            CloseTime = reader.GetTimeOnly(reader.GetOrdinal("CloseTime"));
            Tags = reader.GetString(reader.GetOrdinal("Tags")).Split(';');
            Introduction = reader.GetString(reader.GetOrdinal("Introduction"));
            GoogleMapUrl = reader.GetString(reader.GetOrdinal("GoogleMapUrl"));
            Rating = reader.GetFloat(reader.GetOrdinal("Rating"));
            Created_At = reader.GetDateTime(reader.GetOrdinal("Created_At"));
        }
    }
}
