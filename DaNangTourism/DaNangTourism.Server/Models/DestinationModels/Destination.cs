using DaNangTourism.Server.Helper;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Hosting;
using MySqlConnector;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;

namespace DaNangTourism.Server.Models
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
            Id = reader.GetInt32("DestinationId");
            Name = reader.GetString("Name");
            LocalName = reader.GetString("LocalName");
            Address = reader.GetString("Address");
            Images = reader.GetString("Images").Split(';');
            Cost = reader.GetDouble("Cost");
            OpenTime = reader.GetTimeOnly("OpenTime");
            CloseTime = reader.GetTimeOnly("CloseTime");
            Tags = reader.GetString("Tags").Split(';');
            Introduction = reader.GetString("Introduction");
            GoogleMapUrl = reader.GetString("GoogleMapUrl");
            Rating = reader.GetFloat("Rating");
            Created_At = reader.GetDateTime("Created_At");
        }
    }
}
