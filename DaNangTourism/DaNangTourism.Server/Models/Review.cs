using MySqlConnector;

namespace DaNangTourism.Server.Models
{
    public class Review
    {
        private int _id;
        private string? _content;
        private int _star;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string? Content
        {
            get { return _content; }
            set { _content = value; }
        }
        public int Star
        {
            get { return _star; }
            set { _star = value; }
        }
        public Review() { }
        public Review(int id, string content, int star)
        {
            _id = id;
            _content = content;
            _star = star;
        }
        public Review(MySqlDataReader reader)
        {
            _id = reader.GetInt32("review_id");
            _content = reader.GetString("content");
            _star = reader.GetInt32("star");
        }
    }
}
