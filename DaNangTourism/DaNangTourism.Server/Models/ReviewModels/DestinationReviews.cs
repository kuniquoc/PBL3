using DaNangTourism.Server.ModelBindingConverter;
using MySqlConnector;
using System.Text.Json.Serialization;
namespace DaNangTourism.Server.Models.ReviewModels;

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
        Id = reader.GetInt32(reader.GetOrdinal("Id"));
        Author = reader.GetString(reader.GetOrdinal("Author"));
        Avatar = reader.GetString(reader.GetOrdinal("Avatar"));
        Rating = reader.GetInt32(reader.GetOrdinal("Rating"));
        Comment = reader.GetString(reader.GetOrdinal("Comment"));
        Created_At = reader.GetDateTime(reader.GetOrdinal("Created_At"));
    }
}
