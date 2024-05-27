using MySqlConnector;
using System.Text.Json.Serialization;

namespace DaNangTourism.Server.Models.ReviewModels;

public class ReviewModelToUpdateRating
{
    public int DestinationId { get; set; }
    public int TotalReviewOfDestination { get; set; }
    public int Rating { get; set; }

    public ReviewModelToUpdateRating (MySqlDataReader reader)
    {
        DestinationId = reader.GetInt32(reader.GetOrdinal("DestinationId"));
        TotalReviewOfDestination = reader.GetInt32(reader.GetOrdinal("TotalReviewOfDestination"));
        Rating = reader.GetInt32(reader.GetOrdinal("Rating"));
    }
}
