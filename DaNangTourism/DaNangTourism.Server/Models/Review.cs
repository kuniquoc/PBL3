using MySqlConnector;

namespace DaNangTourism.Server.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public int Star { get; set; }
        public Review() { }
        public Review(MySqlDataReader reader)
        {
            Id = reader.GetInt32("review_id");
            Content = reader.GetString("content");
            Star = reader.GetInt32("star");
        }
    }
}
