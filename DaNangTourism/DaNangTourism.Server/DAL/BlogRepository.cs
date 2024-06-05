using DaNangTourism.Server.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR.Protocol;
using MySqlConnector;
using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;

namespace DaNangTourism.Server.DAL
{
  public interface IBlogRepository
  {
    List<BlogHome> Get5MostView();
    int IncreaseView(params MySqlParameter[] parameters);
    List<BlogPage> GetBlogPage(string sql, List<MySqlParameter> parameters);
    int GetBlogCount(string sql, List<MySqlParameter> parameters);
    List<BlogRandom> GetRandomBlog(string filter, List<MySqlParameter> parameters);
    BlogDetail? GetBlogDetail(int id);
    int AddBlog(BlogAdd blogAdd, int uid);
    BlogForEdit GetBlogToUpdate(int id);
    bool CheckBlogBelongToUser(int blogId, int uid);
    void UpdateBlog(BlogAdd blogAdd, int id);
    void DeleteBlog(int id);
    List<BlogList> GetBlogList(string sql, List<MySqlParameter> parameters);
    void UpdateStatus(int blogId, BlogStatus status);
  }
  public class BlogRepository : IBlogRepository
  {
    private readonly string _connectionString;
    public BlogRepository(string connectionString)
    {
      _connectionString = connectionString;
    }

    // lấy 5 blog mới nhất
    public List<BlogHome> Get5MostView()
    {
      string sql = "SELECT blog_id, type, title, image, " +
          "(SELECT full_name FROM users WHERE users.user_id = blogs.user_id) AS author, " +
          "created_at FROM blogs ORDER BY views desc LIMIT 5";
      using (var con = new MySqlConnection(_connectionString))
      {
        using (var cmd = new MySqlCommand(sql, con))
        {
          con.Open();
          using (var reader = cmd.ExecuteReader())
          {
            var blogHomes = new List<BlogHome>();
            while (reader.Read())
            {
              var blogHome = new BlogHome(reader);
              blogHomes.Add(blogHome);
            }
            return blogHomes;
          }
        }
      }
    }

    // tăng view
    public int IncreaseView(params MySqlParameter[] parameters)
    {
      string sql = "update blogs set views = views + 1 where blog_id = @id";
      using (var con = new MySqlConnection(_connectionString))
      {
        con.Open();
        using (var cmd = new MySqlCommand(sql, con))
        {
          cmd.Parameters.AddRange(parameters);
          return cmd.ExecuteNonQuery();
        }
      }
    }


    // lấy blog theo page
    public List<BlogPage> GetBlogPage(string sql, List<MySqlParameter> parameters)
    {

      using (var con = new MySqlConnection(_connectionString))
      {
        con.Open();
        using (var cmd = new MySqlCommand(sql, con))
        {
          cmd.Parameters.AddRange(parameters.ToArray());
          using (var reader = cmd.ExecuteReader())
          {
            var blogPages = new List<BlogPage>();
            while (reader.Read())
            {
              blogPages.Add(new BlogPage(reader));
            }
            return blogPages;
          }
        }
      }
    }

    public int GetBlogCount(string sql, List<MySqlParameter> parameters)
    {
      using (var con = new MySqlConnection(_connectionString))
      {
        con.Open();
        using (var cmd = new MySqlCommand(sql, con))
        {
          cmd.Parameters.AddRange(parameters.ToArray());
          return Convert.ToInt32(cmd.ExecuteScalar());
        }
      }
    }

    // lấy random blog
    public List<BlogRandom> GetRandomBlog(string filter, List<MySqlParameter> parameters)
    {
      string sql = "Select blog_id, title, type, image, user_id, created_at, (SELECT full_name FROM users WHERE users.user_id = blogs.user_id) as author" +
          " from blogs order by rand()" + filter;

      using (var con = new MySqlConnection(_connectionString))
      {
        con.Open();
        using (var cmd = new MySqlCommand(sql, con))
        {
          cmd.Parameters.AddRange(parameters.ToArray());
          using (var reader = cmd.ExecuteReader())
          {
            var blogRandoms = new List<BlogRandom>();
            while (reader.Read())
            {
              blogRandoms.Add(new BlogRandom(reader));
            }
            return blogRandoms;
          }
        }
      }
    }


    //lấy blog dựa vào id // để hiển thị bài blog
    public BlogDetail? GetBlogDetail(int id)
    {
      string sql = "SELECT blog_id, title, type, blogs.created_at as created_at, views, content, blogs.user_id AS id, full_name AS name, avatar_url AS avatar " +
          "FROM blogs INNER JOIN users ON blogs.user_id = users.user_id WHERE blog_id = @id";
      var parameter = new MySqlParameter("@id", id);

      using (var con = new MySqlConnection(_connectionString))
      {
        con.Open();
        using (var cmd = new MySqlCommand(sql, con))
        {
          cmd.Parameters.Add(parameter);
          using (var reader = cmd.ExecuteReader())
          {
            if (reader.Read())
            {
              return new BlogDetail(reader);
            }
            return null;
          }
        }
      }
    }

    // thêm blog
    public int AddBlog(BlogAdd blogAdd, int uid)
    {
      string sql = "INSERT INTO blogs(user_id, title, type, image, introduction, content, status) " +
          "VALUES (@uid, @title, @type, @image, @introduction, @content, @status); SELECT LAST_INSERT_ID();";
      MySqlParameter[] parameters =
      {
                new ("@uid", uid),
                new ("@title", blogAdd.title),
                new ("@type", blogAdd.type),
                new ("@image", blogAdd.image),
                new ("@introduction", blogAdd.introduction),
                new ("@content", blogAdd.content),
                new ("@status", BlogStatus.pending),
            };
      using (var con = new MySqlConnection(_connectionString))
      {
        con.Open();
        using (var cmd = new MySqlCommand(sql, con))
        {
          cmd.Parameters.AddRange(parameters);
          return Convert.ToInt32(cmd.ExecuteScalar());
        }
      }
    }

    // nhận blog cần sửa
    public BlogForEdit GetBlogToUpdate(int id)
    {
      string sql = "SELECT title, type, image, introduction, content FROM blogs WHERE blog_id = @blogID";
      var parameter = new MySqlParameter("@blogID", id);
      using (var con = new MySqlConnection(_connectionString))
      {
        con.Open();
        using (var cmd = new MySqlCommand(sql, con))
        {
          cmd.Parameters.Add(parameter);
          using (var reader = cmd.ExecuteReader())
          {
            if (reader.Read())
            {
              return new BlogForEdit(reader);
            }
            throw new Exception("Blog doesn't exist");
          }
        }
      }
    }

    // kiểm tra tác giả bài viết
    public bool CheckBlogBelongToUser(int blogId, int uid)
    {
      string sql = "select * from blogs where blog_id = @blogID and user_id = @uid";
      MySqlParameter[] parameters =
      {
                new ("@blogID", blogId),
                new ("@uid", uid)
            };
      using (var con = new MySqlConnection(_connectionString))
      {
        con.Open();
        using (var cmd = new MySqlCommand(sql, con))
        {
          cmd.Parameters.AddRange(parameters);
          using (var reader = cmd.ExecuteReader())
          {
            if (reader.Read())
            {
              return true;
            }
            return false;
          }
        }
      }
    }

    // sửa blog
    public void UpdateBlog(BlogAdd blogAdd, int id)
    {
      string sql = "UPDATE blogs SET title = @title, type = @type, image = @image, " +
          "introduction = @introduction, content = @content WHERE blog_id = @blogID;";
      MySqlParameter[] parameters =
      {
                new ("@title", blogAdd.title),
                new ("@type", blogAdd.type),
                new ("@image", blogAdd.image),
                new ("@introduction", blogAdd.introduction),
                new ("@content", blogAdd.content),
                new ("@blogID", id),
            };
      using (var con = new MySqlConnection(_connectionString))
      {
        con.Open();
        using (var cmd = new MySqlCommand(sql, con))
        {
          cmd.Parameters.AddRange(parameters);
          cmd.ExecuteNonQuery();
        }
      }
    }

    // xóa blog
    public void DeleteBlog(int id)
    {
      string sql = "DELETE FROM blogs WHERE blog_id = @blogID";
      MySqlParameter parameter = new MySqlParameter("@blogID", id);
      using (var con = new MySqlConnection(_connectionString))
      {
        con.Open();
        using (var cmd = new MySqlCommand(sql, con))
        {
          cmd.Parameters.Add(parameter);
          cmd.ExecuteNonQuery();
        }
      }
    }

    // lấy danh sách blog cho admin
    public List<BlogList> GetBlogList(string sql, List<MySqlParameter> parameters)
    {

      using (var con = new MySqlConnection(_connectionString))
      {
        con.Open();
        using (var cmd = new MySqlCommand(sql, con))
        {
          cmd.Parameters.AddRange(parameters.ToArray());
          using (var reader = cmd.ExecuteReader())
          {
            var blogLists = new List<BlogList>();
            while (reader.Read())
            {
              blogLists.Add(new BlogList(reader));
            }
            return blogLists;
          }
        }
      }
    }

    // cập nhật trạng thái Blog
    public void UpdateStatus(int blogId, BlogStatus status)
    {
      string sql = "UPDATE blogs SET status = @status WHERE blog_id = @blogID";
      MySqlParameter[] parameters =
      {
        new ("@blogID", blogId),
        new ("@status", status)
      };
      using (var con = new MySqlConnection(_connectionString))
      {
        con.Open();
        using (var cmd = new MySqlCommand(sql, con))
        {
          cmd.Parameters.AddRange(parameters);
          cmd.ExecuteNonQuery();
        }
      }
    }

  }
}
