using DaNangTourism.Server.DAL;
using MySqlConnector;

namespace DaNangTourism.Server.Models
{
    public class BlogRandomFilter
    {
        public int limit { get; set; } = 5;
    }
    public class BlogPageFilter
    {
        public int page { get; set; } = 1;
        public int limit { get; set; } = 5;
        public string? search { get; set; }
        public string? sortBy { get; set; } = "created_at";
        public string? sortType { get; set; } = "desc";
    }
    public class BlogListAdminFilter
    {
        public int page { get; set; } = 1;
        public int limit { get; set; } = 12;
        public string? search { get; set; }
        public string? type { get; set; }
        public Status status { get; set; }
        public string? sortBy { get; set; } = "created_at";
        public string? sortType { get; set; } = "desc";
    }
    public enum Status
    {
        pending,
        published,
        rejected
    }
    public class BlogHome
    {
        public int id { get; set; }
        public string? type { get; set; } = "";
        public string? title { get; set; } = "";
        public string? image { get; set; } = "";
        public string? authorName { get; set; } = "";
        public DateTime created_at { get; set; } = DateTime.Now;
        public BlogHome() { }
        public BlogHome(MySqlDataReader reader)
        {
            id = reader.GetInt32("blog_id");
            type = reader.GetString("type");
            title = reader.GetString("title");
            image = reader.GetString("image");
            created_at = reader.GetDateTime("created_at");
        }
    }
    public class BlogPage
    {
        public int id { get; set; }
        public string? title { get; set; } = "";
        public string? image { get; set; } = "";
        public string? type { get; set; } = "";
        public Author? author { get; set; }
        public DateTime created_at { get; set; } = DateTime.Now;
        public int views { get; set; } = 0;
        public string? introduction { get; set; } = "";
        public BlogPage() { }
        public BlogPage(MySqlDataReader reader)
        {
            id = reader.GetInt32("blog_id");
            title = reader.GetString("title");
            image = reader.GetString("image");
            type = reader.GetString("type");
            created_at = reader.GetDateTime("created_at");
            views = reader.GetInt32("views");
            introduction = reader.GetString("introduction");
        }
    }
    public class BlogRandom
    {
        public int id { get; set; }
        public string? title { get; set; } = "";
        public string? type { get; set; } = "";
        public string? image { get; set; } = "";
        public string? authorName { get; set; } = "";
        public DateTime created_at { get; set; } = DateTime.Now;
        public BlogRandom() { }
        public BlogRandom(MySqlDataReader reader)
        {
            id = reader.GetInt32("blog_id");
            title = reader.GetString("title");
            type = reader.GetString("type");
            image = reader.GetString("image");
            created_at = reader.GetDateTime("created_at");
        }
    }
    public class BlogDetail
    {
        public int id { get; set; }
        public string? title { get; set; } = "";
        public string? type { get; set; } = "";
        public Author? author { get; set; }
        public DateTime created_at { get; set; } = DateTime.Now;    
        public int views { get; set; }
        public string? content { get; set; } = "";
        public BlogDetail() { }
        public BlogDetail(MySqlDataReader reader)
        {
            id = reader.GetInt32("blog_id");
            title = reader.GetString("title");
            type = reader.GetString("type");
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
    public class BlogList
    {
        public int id { get; set; }
        public string title { get; set; } = "";
        public string? type { get; set; } = "";
        public string? authorName { get; set; } 
        public DateTime created_at { get; set; } = DateTime.Now;
        public Status status { get; set;}
        public BlogList() { }
        public BlogList(MySqlDataReader reader)
        {
            id = reader.GetInt32("blog_id");
            title = reader.GetString("title");
            type = reader.GetString("type");
            created_at = reader.GetDateTime("created_at");
            status = Enum.Parse<Status>(reader.GetString("status"));
        }
    }
}
