using DaNangTourism.Server.DAL;
using MySqlConnector;

namespace DaNangTourism.Server.Models
{
    public class Blog
    {
        public int id { get; set; }
        public string? title { get; set; }
        public DateTime created_at { get; set; }
        //public Blog() { }
    }
    public class BlogHome : Blog
    {
        public string? image { get; set; }
        public string? authorName { get; set; }
        public BlogHome() { }
        public BlogHome(MySqlDataReader reader)
        {
            int uid = reader.GetInt32("uid");
            id = reader.GetInt32("id");
            title = reader.GetString("title");
            image = reader.GetString("image");
            authorName = AccountDAO.Instance.getAuthorName(uid);
            created_at = reader.GetDateTime("created_at");
        }
    }
    public class BlogPage : Blog
    {
        public string? type { get; set; }
        public string? image { get; set; }
        public Author? author { get; set; }
        public int views { get; set; }
        public string? introduction { get; set; }
        public BlogPage() { }
        public BlogPage(MySqlDataReader reader)
        {
            int uid = reader.GetInt32("uid");
            id = reader.GetInt32("id");
            title = reader.GetString("title");
            image = reader.GetString("image");
            type = reader.GetString("type");
            author = AccountDAO.Instance.getAuthorById(uid);
            created_at = reader.GetDateTime("created_at");
            views = reader.GetInt32("views");
            introduction = reader.GetString("introduction");
        }
    }
    public class BlogRandom : Blog
    {
        public string? type { get; set; }
        public string? image { get; set; }
        public string? authorName { get; set; }
        public BlogRandom() { }
        public BlogRandom(MySqlDataReader reader)
        {
            int uid = reader.GetInt32("uid");
            id = reader.GetInt32("id");
            title = reader.GetString("title");
            type = reader.GetString("type");
            image = reader.GetString("image");
            authorName = AccountDAO.Instance.getAuthorName(uid);
            created_at = reader.GetDateTime("created_at");
        }
    }
    public class BlogDetail : Blog
    {
        public string? type { get; set; }
        public Author? author { get; set; }
        public int views { get; set; }
        public string? content { get; set; }
        public BlogDetail() { }
        public BlogDetail(MySqlDataReader reader)
        {
            int uid = reader.GetInt32("uid");
            id = reader.GetInt32("id");
            title = reader.GetString("title");
            type = reader.GetString("type");
            author = AccountDAO.Instance.getAuthorById(uid);
            created_at = reader.GetDateTime("created_at");
            views = reader.GetInt32("views");
            content = reader.GetString("content");
        }
    }        
}
