using DaNangTourism.Server.Helper;
using MySqlConnector;
using System.Text.Json.Serialization;
namespace DaNangTourism.Server.Models
{
    public class DestinationReviews
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }
        [JsonPropertyName("page")]
        public int Page { get; set; }
        [JsonPropertyName("limit")]
        public int Limit { get; set; }
        public List<DestinationReview> Items { get; set; } = new List<DestinationReview>();
    }
    public class DestinationReview
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("author")]
        public string Author { get; set; }
        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }
        [JsonPropertyName("rating")]
        public int Rating { get; set; }
        [JsonPropertyName("comment")]
        public string Comment { get; set; }
        [JsonPropertyName("created_at")]
        [JsonConverter(typeof(ConvertToISO8061DateTime))]
        public DateTime Created_At { get; set; }
        public DestinationReview(MySqlDataReader reader)
        {
            Id = reader.GetInt32(0);
            Author = reader.GetString(1);
            Avatar = reader.GetString(2);
            Rating = reader.GetInt32(3);
            Comment = reader.GetString(4);
            Created_At = reader.GetDateTime(5);
        }
    }
}
