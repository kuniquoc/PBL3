using DaNangTourism.Server.DAL;
using DaNangTourism.Server.Models;
using DaNangTourism.Server.Models.DestinationModels;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace DaNangTourism.Server.Services
{
  public interface IBlogService
  {
    List<BlogHome> Get5MostView();
    int IncreaseView(int id);
    BlogPageData GetBlogPage(BlogPageFilter blogPF);
    List<BlogRandom> GetRandomBlog(BlogRandomFilter blogRF);
    BlogDetail? GetBlogDetail(int id, bool isAdmin = false, int uid = 0);
    int AddBlog(BlogAdd blogAdd, int uid);
    BlogForEdit GetBlogToUpdate(int id);
    bool CheckBlogBelongToUser(int blogId, int uid);
    void UpdateBlog(BlogAdd blogAdd, int id);
    void DeleteBlog(int id);
    BLogListData GetBlogList(BlogListAdminFilter blogLAF, int uid = 0);
    void UpdateStatus(int blogId, BlogStatus status);
  }
  public class BlogService : IBlogService
  {
    private readonly IBlogRepository _blogRepository;
    public BlogService(IBlogRepository blogRepository)
    {
      _blogRepository = blogRepository;
    }

    // tăng view
    public int IncreaseView(int id)
    {
      MySqlParameter parameter = new MySqlParameter("@id", id);
      return _blogRepository.IncreaseView(parameter);
    }

    // 5 bài có lượt xem cao nhất
    public List<BlogHome> Get5MostView()
    {
      return _blogRepository.Get5MostView();
    }


    // lấy Blog đặt vào giao diện chính
    public BlogPageData GetBlogPage(BlogPageFilter blogPF)
    {
      BlogPageData blogPageData = new BlogPageData();

      StringBuilder sql = new StringBuilder();
      sql.Append("SELECT blog_id, title, image, type, blogs.created_at as created_at, views, introduction, blogs.user_id AS id, full_name AS name, avatar_url AS avatar ");
      sql.Append("FROM blogs INNER JOIN users ON blogs.user_id = users.user_id");
      List<MySqlParameter> parameters = new List<MySqlParameter>();
      sql.Append(" WHERE status = 'published'");
      if (!blogPF.search.IsNullOrEmpty())
      {
        sql.Append(" AND title LIKE @title");
        parameters.Add(new MySqlParameter("@title", "%" + blogPF.search + "%"));
      }

      // xử lý order by
      sql.Append(" ORDER BY " + blogPF.sortBy);

      sql.Append(" " + blogPF.sortType);


      // xử lý blogPageData
      // xử lý total
      StringBuilder countSql = new StringBuilder();
      countSql.Append("SELECT COUNT(*) FROM (" + sql + ") AS subquery");
      blogPageData.total = _blogRepository.GetBlogCount(countSql.ToString(), parameters);

      // xử lý limit
      blogPageData.limit = blogPF.limit;
      sql.Append(" limit @limit");
      parameters.Add(new MySqlParameter("@limit", blogPF.limit));

      // xử lý offset (page)
      blogPageData.page = blogPF.page;
      sql.Append(" offset @offset");
      parameters.Add(new MySqlParameter("@offset", (blogPF.page - 1) * blogPF.limit));

      // xử lý item 
      blogPageData.items = _blogRepository.GetBlogPage(sql.ToString(), parameters);


      return blogPageData;
    }

    // random blog
    public List<BlogRandom> GetRandomBlog(BlogRandomFilter blogRF)
    {
      return _blogRepository.GetRandomBlog(blogRF.limit);
    }


    // chi tiết blog
    public BlogDetail? GetBlogDetail(int id, bool isAdmin = false, int uid = 0)
    {
      bool isAdminOrOwner = isAdmin;
      if (uid != 0 && !isAdmin)
      {
        isAdminOrOwner = _blogRepository.CheckBlogBelongToUser(id, uid);
      }
      return _blogRepository.GetBlogDetail(id, isAdminOrOwner);
    }


    // thêm Blog
    public int AddBlog(BlogAdd blogAdd, int uid)
    {
      return _blogRepository.AddBlog(blogAdd, uid);
    }

    // nhận blog để update
    public BlogForEdit GetBlogToUpdate(int id)
    {
      return _blogRepository.GetBlogToUpdate(id);
    }

    // kiểm tra tác giả Blog
    public bool CheckBlogBelongToUser(int blogId, int uid)
    {
      return _blogRepository.CheckBlogBelongToUser(blogId, uid);
    }

    // cập nhật Blog
    public void UpdateBlog(BlogAdd blogAdd, int id)
    {
      _blogRepository.UpdateBlog(blogAdd, id);
    }

    // xóa Blog
    public void DeleteBlog(int id)
    {
      _blogRepository.DeleteBlog(id);
    }


    // lấy blog list dành cho admin
    public BLogListData GetBlogList(BlogListAdminFilter blogLAF, int uid = 0)
    {
      BLogListData bLogListData = new BLogListData();

      StringBuilder sql = new StringBuilder();
      sql.Append("SELECT blog_id, title, type, created_at, views, status, " +
          "(SELECT full_name FROM users WHERE users.user_id = blogs.user_id) AS author FROM blogs");
      List<MySqlParameter> parameters = new List<MySqlParameter>();

      // xử lý where
      if (!blogLAF.search.IsNullOrEmpty())
      {
        if (sql.ToString().Contains("where"))
        {
          sql.Append(" AND title LIKE @title");
        }
        else
        {
          sql.Append(" WHERE title LIKE @title");
        }
        parameters.Add(new MySqlParameter("@title", "%" + blogLAF.search + "%"));
      }
      if (blogLAF.type != BlogType.all)
      {
        if (sql.ToString().Contains("where"))
        {
          sql.Append(" AND type = @type");
        }
        else
        {
          sql.Append(" WHERE type = @type");
        }
        parameters.Add(new MySqlParameter("@type", blogLAF.type.ToString()));
      }
      if (blogLAF.status != BlogStatus.all)
      {
        if (sql.ToString().Contains("where"))
        {
          sql.Append(" AND status = @status");
        }
        else
        {
          sql.Append(" WHERE status = @status");
        }
        parameters.Add(new MySqlParameter("@status", blogLAF.status.ToString()));
      }

      if (uid != 0)
      {
        if (sql.ToString().Contains("where"))
        {
          sql.Append(" AND user_id = @uid");
        }
        else
        {
          sql.Append(" WHERE user_id = @uid");
        }
        parameters.Add(new MySqlParameter("@uid", uid));
      }

      // xử lý order by

      sql.Append(" ORDER BY " + blogLAF.sortBy);

      sql.Append(" " + blogLAF.sortType);

      // xử lý blogListData
      // xử lý total
      StringBuilder countSql = new StringBuilder();
      countSql.Append("SELECT COUNT(*) FROM (" + sql + ") AS subquery");
      bLogListData.total = _blogRepository.GetBlogCount(countSql.ToString(), parameters);

      // xử lý limit
      bLogListData.limit = blogLAF.limit;
      sql.Append(" limit @limit");
      parameters.Add(new MySqlParameter("@limit", blogLAF.limit));

      // xử lý offset (page)
      bLogListData.page = blogLAF.page;
      sql.Append(" offset @offset");
      parameters.Add(new MySqlParameter("@offset", (blogLAF.page - 1) * blogLAF.limit));

      // xử lý item 
      bLogListData.items = _blogRepository.GetBlogList(sql.ToString(), parameters);

      return bLogListData;

    }

    // cập nhật trạng thái Blog
    public void UpdateStatus(int blogId, BlogStatus status)
    {
      _blogRepository.UpdateStatus(blogId, status);
    }

  }
}
