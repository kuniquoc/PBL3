using MySqlConnector;

namespace DaNangTourism.Server.Models
{
    public class Blog
    {
        public int BlogID { get; set; }
        public int UserID { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime PostTime { get; set; }
        public string? BlogView { get; set; }
        public Blog() { }
        public Blog(MySqlDataReader reader)
        {
            BlogID = reader.GetInt32(0);
            UserID = reader.GetInt32(1);
            Title = reader.GetString(2);
            Content = reader.GetString(3);
            PostTime= reader.GetDateTime(4);
            BlogView = reader.GetString(5);
        }
    }
}
