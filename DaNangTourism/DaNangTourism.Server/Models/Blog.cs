using DaNangTourism.Server.DAL;
using DaNangTourism.Server.Helper;
using DaNangTourism.Server.ModelBindingConverter;
using DaNangTourism.Server.Models.ScheduleModels;
using MySqlConnector;
using System.ComponentModel;
using System.Globalization;
using System.Text.Json.Serialization;

namespace DaNangTourism.Server.Models
{
    [TypeConverter(typeof(BlogStatusTypeConverter))]
    public enum BlogStatus
    {
        all,
        pending,
        published,
        rejected
    }
    [TypeConverter(typeof(BlogTypeTypeConverter))]
    public enum BlogType
    {
        all,
        places,
        tips
    }

    public class BlogHome
    {
        public int id { get; set; }
        [JsonConverter(typeof(EnumToStringJsonConverter<BlogType>))]
        public BlogType type { get; set; }
        public string title { get; set; }
        public string image { get; set; }
        public string author { get; set; }
        [JsonPropertyName("createdAt")]
        [JsonConverter(typeof(ConvertToISO8061DateTime))]
        public DateTime createdAt { get; set; } = DateTime.Now;
        public BlogHome(MySqlDataReader reader)
        {
            id = reader.GetInt32("blog_id");
            type = Enum.Parse<BlogType>(reader.GetString("type"));
            title = reader.GetString("title");
            image = reader.GetString("image");
            author = reader.GetString("author");
            createdAt = reader.GetDateTime("created_at");
        }
    }
    public class BlogPageFilter
    {
        public int page { get; set; } = 1;
        public int limit { get; set; } = 5;
        public string search { get; set; } = "";
        public string sortBy { get; set; } = "created_at";
        public string sortType { get; set; } = "desc";
        public void Sanitization()
        {
            sortBy = DataSanitization.RemoveSpecialCharacters(sortBy);
            sortType = DataSanitization.RemoveSpecialCharacters(sortType);
            if (page < 1)
            {
                page = 1;
            }
            if (limit < 1)
            {
                limit = 5;
            }
            search = DataSanitization.RemoveSpecialCharacters(search);

            if (!sortBy.Equals("title") && !sortBy.Equals("created_at") && !sortBy.Equals("views")) sortBy = "created_at";

            if (!sortType.Equals("asc") && !sortType.Equals("desc")) sortType = "desc";
        }
    }
    public class BlogPage
    {
        public int id { get; set; }
        public string title { get; set; } = "";
        public string image { get; set; } = "";
        [JsonConverter(typeof(EnumToStringJsonConverter<BlogType>))]
        public BlogType type { get; set; } = BlogType.all;
        public Author author { get; set; }
        [JsonPropertyName("createdAt")]
        [JsonConverter(typeof(ConvertToISO8061DateTime))]
        public DateTime createdAt { get; set; } = DateTime.Now;
        public int views { get; set; } = 0;
        public string introduction { get; set; } = "";
        public BlogPage(MySqlDataReader reader)
        {
            id = reader.GetInt32("blog_id");
            title = reader.GetString("title");
            image = reader.GetString("image");
            type = Enum.Parse<BlogType>(reader.GetString("type"));
            createdAt = reader.GetDateTime("created_at");
            views = reader.GetInt32("views");
            introduction = reader.GetString("introduction");
            author = new Author(reader);
        }
    }
    public class BlogPageData
    {
        public int total { get; set; }
        public int page { get; set; } = 1;
        public int limit { get; set; } = 5;
        public List<BlogPage> items { get; set; }
        public BlogPageData()
        {
            items = new List<BlogPage>();
        }
    }

    public class BlogRandomFilter
    {
        public int limit { get; set; } = 5;
    }

    public class BlogRandom
    {
        public int id { get; set; }
        public string title { get; set; } = "";
        [JsonConverter(typeof(EnumToStringJsonConverter<BlogType>))]
        public BlogType type { get; set; } = BlogType.all;
        public string image { get; set; } = "";
        public string author { get; set; } = "";
        [JsonPropertyName("createdAt")]
        [JsonConverter(typeof(ConvertToISO8061DateTime))]
        public DateTime createdAt { get; set; } = DateTime.Now;
        public BlogRandom() { }
        public BlogRandom(MySqlDataReader reader)
        {
            id = reader.GetInt32("blog_id");
            title = reader.GetString("title");
            type = Enum.Parse<BlogType>(reader.GetString("type"));
            image = reader.GetString("image");
            createdAt = reader.GetDateTime("created_at");
            author = reader.GetString("author");
        }
    }

    public class BlogDetail
    {
        public int id { get; set; }
        public string title { get; set; } = "";
        [JsonConverter(typeof(EnumToStringJsonConverter<BlogType>))]
        public BlogType type { get; set; } = BlogType.all;
        public Author author { get; set; }
        [JsonPropertyName("createdAt")]
        [JsonConverter(typeof(ConvertToISO8061DateTime))]
        public DateTime createdAt { get; set; } = DateTime.Now;
        public int views { get; set; }
        public string content { get; set; } = "";
        public BlogDetail(MySqlDataReader reader)
        {
            id = reader.GetInt32("blog_id");
            title = reader.GetString("title");
            type = Enum.Parse<BlogType>(reader.GetString("type"));
            createdAt = reader.GetDateTime("created_at");
            views = reader.GetInt32("views");
            content = reader.GetString("content");
            author = new Author(reader);
        }
    }

    public class BlogListAdminFilter
    {
        public int page { get; set; } = 1;
        public int limit { get; set; } = 12;
        public string search { get; set; } = "";
        [JsonConverter(typeof(EnumToStringJsonConverter<BlogType>))]
        public BlogType type { get; set; } = BlogType.all;
        [JsonConverter(typeof(EnumToStringJsonConverter<BlogStatus>))]
        public BlogStatus status { get; set; } = BlogStatus.all;
        public string sortBy { get; set; } = "created_at";
        public string sortType { get; set; } = "desc";
        public void Sanitization()
        {
            sortBy = DataSanitization.RemoveSpecialCharacters(sortBy);
            sortType = DataSanitization.RemoveSpecialCharacters(sortType);
            if (page < 1)
            {
                page = 1;
            }
            if (limit < 1)
            {
                limit = 12;
            }
            search = DataSanitization.RemoveSpecialCharacters(search);
            if (!sortBy.Equals("title") && !sortBy.Equals("created_at") && !sortBy.Equals("views")) sortBy = "created_at";
            if (!sortType.Equals("asc") && !sortType.Equals("desc")) sortType = "desc";
        }
    }

    public class BlogList
    {
        public int id { get; set; }
        public string title { get; set; } = "";
        [JsonConverter(typeof(EnumToStringJsonConverter<BlogType>))]
        public BlogType type { get; set; } = BlogType.all;
        public string author { get; set; }
        [JsonPropertyName("createdAt")]
        [JsonConverter(typeof(ConvertToISO8061DateTime))]
        public DateTime createdAt { get; set; } = DateTime.Now;
        [JsonConverter(typeof(EnumToStringJsonConverter<BlogStatus>))]
        public BlogStatus status { get; set; }
        public int views { get; set; } = 0;
        public BlogList(MySqlDataReader reader)
        {
            id = reader.GetInt32("blog_id");
            title = reader.GetString("title");
            type = Enum.Parse<BlogType>(reader.GetString("type"));
            author = reader.GetString("author");
            createdAt = reader.GetDateTime("created_at");
            status = Enum.Parse<BlogStatus>(reader.GetString("status"));
            views = reader.GetInt32("views");
        }
    }
    public class BLogListData
    {
        public int total { get; set; }
        public int page { get; set; } = 1;
        public int limit { get; set; } = 12;
        public List<BlogList>? items { get; set; }
        public BLogListData()
        {
            items = new List<BlogList>();
        }
    }

    public class BlogAdd
    {
        public string title { get; set; }
        [JsonConverter(typeof(EnumToStringJsonConverter<BlogType>))]
        public BlogType type { get; set; }
        public string image { get; set; }
        public string introduction { get; set; }
        public string content { get; set; }
    }

    public class BlogForEdit
    {
        public string title { get; set; }
        [JsonConverter(typeof(EnumToStringJsonConverter<BlogType>))]
        public BlogType type { get; set; }
        public string image { get; set; }
        public string introduction { get; set; }
        public string content { get; set; }

        public BlogForEdit(MySqlDataReader reader)
        {
            title = reader.GetString("title");
            type = Enum.Parse<BlogType>(reader.GetString("type"));
            image = reader.GetString("image");
            introduction = reader.GetString("introduction");
            content = reader.GetString("content");
        }
    }

    public class BlogReturn<T>
    {
        public int status { get; set; } = 200;
        public string message { get; set; } = "Sucess";
        public T data { get; set; }
        public BlogReturn(T blog)
        {
            this.data = blog;
        }
    }
}
