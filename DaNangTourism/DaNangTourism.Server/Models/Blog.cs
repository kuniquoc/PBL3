using MySqlConnector;

namespace DaNangTourism.Server.Models
{
    public class Blog
    {
        private int _blogID;
        private int _userID;
        private string? _title;
        private string? _content;
        private DateTime _postTime;
        private string? _blogView;

        public int BlogID { get { return _blogID; } set { _blogID = value; } }
        public int UserID { get { return _userID; } set { _userID = value; } }
        public string? Title { get { return _title; } set { _title = value; } }
        public string? Content { get { return _content; } set { _content = value; } }
        public DateTime PostTime { get { return _postTime; } set { _postTime = value; } }
        public string? BlogView { get { return _blogView; } set { _blogView = value; } }
        public Blog() { }
        public Blog(int blogID, int userID,string? title, string? content, DateTime postTime, string? blogView)
        {
            _blogID = blogID;
            _userID = userID;
            _title = title;
            _content = content;
            _postTime = postTime;
            _blogView = blogView;
        }
        public Blog(MySqlDataReader reader)
        {
            _blogID = reader.GetInt32(0);
            _userID = reader.GetInt32(1);
            _title = reader.GetString(2);
            _content = reader.GetString(3);
            _postTime= reader.GetDateTime(4);
            _blogView = reader.GetString(5);
        }
    }
}
