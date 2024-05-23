using DaNangTourism.Server.DAL;
using MySqlConnector;

namespace DaNangTourism.Server.Models
{
    public enum Status
    {
        pending,
        published,
        rejected
    }
    public class Blog
    {
        public int id { get; set; }
        public string? title { get; set; }
        public DateTime created_at { get; set; }
    }
    public class BlogHome : Blog
    {
        public string? image { get; set; }
        public string? authorName { get; set; }
        public BlogHome() { }
        public BlogHome(MySqlDataReader reader)
        {
            int uid = reader.GetInt32("user_id");
            id = reader.GetInt32("blog_id");
            title = reader.GetString("title");
            image = reader.GetString("image");
            authorName = AccountDAO.Instance.GetAccountById(uid).Name;
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
            int uid = reader.GetInt32("user_id");
            id = reader.GetInt32("blog_id");
            title = reader.GetString("title");
            image = reader.GetString("image");
            type = reader.GetString("type");
            author = new Author(AccountDAO.Instance.GetAccountById(uid));
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
            int uid = reader.GetInt32("user_id");
            id = reader.GetInt32("blog_id");
            title = reader.GetString("title");
            type = reader.GetString("type");
            image = reader.GetString("image");
            authorName = AccountDAO.Instance.GetAccountById(uid).Name;
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
            int uid = reader.GetInt32("user_id");
            id = reader.GetInt32("blog_id");
            title = reader.GetString("title");
            type = reader.GetString("type");
            author = new Author(AccountDAO.Instance.GetAccountById(uid));
            created_at = reader.GetDateTime("created_at");
            views = reader.GetInt32("views");
            content = reader.GetString("blog_view");
        }
    }   
    public class BlogAdd
    {
        public string? title { get; set; }
        public string? type { get; set; }
        public string? image { get; set; }
        public string? introduction { get; set; }
        public string? content { get; set; }
    }
    public class BlogList : Blog
    {
        public string? type { get; set;}
        public string? authorName { get; set; } 
        public Status status { get; set;}
        public BlogList(MySqlDataReader reader)
        {
            int uid = reader.GetInt32("user_id");
            id = reader.GetInt32("blog_id");
            title = reader.GetString("title");
            type = reader.GetString("type");
            created_at = reader.GetDateTime("created_at");
            status = Enum.Parse<Status>(reader.GetString("status"));
            authorName = AccountDAO.Instance.GetAccountById(uid).Name;
        }
    }
}
