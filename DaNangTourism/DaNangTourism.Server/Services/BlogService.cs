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
        BlogDetail? GetBlogDetail(int id);
        int AddBlog(BlogAdd blogAdd, int uid);
        bool CheckBlogBelongToUser(int blogId, int uid);
        BlogAdd? UpdateBlog(BlogAdd blogAdd, int id);
        void DeleteBlog(int id);
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
            return _blogRepository.increaseView(parameter);
        }

        // 5 bài có lượt xem cao nhất
        public List<BlogHome> Get5MostView()
        {
            return _blogRepository.get5MostView();
        }


        // lấy Blog đặt vào giao diện chính
        public BlogPageData GetBlogPage(BlogPageFilter blogPF)
        {
            BlogPageData blogPageData = new BlogPageData();

            StringBuilder sql = new StringBuilder();
            sql.Append("Select blog_id, title, image, type, user_id, created_at, views, introduction from blogs");
            List<MySqlParameter> parameters = new List<MySqlParameter>();

            if (!blogPF.search.IsNullOrEmpty())
            {
                sql.Append(" where title like @title");
                parameters.Add(new MySqlParameter("@title", "'%" + blogPF.search + "%'"));
            }

            if (!blogPF.sortBy.IsNullOrEmpty())
            {
                sql.Append(" order by @sortBy");
                parameters.Add(new MySqlParameter("@sortBy", blogPF.sortBy));
            }
            else
            {
                sql.Append(" order by created_at");
            }

            if (!blogPF.sortType.IsNullOrEmpty())
            {
                sql.Append(" @sortType");
                parameters.Add(new MySqlParameter("@sortType", blogPF.sortType));
            }
            else
            {
                sql.Append(" desc");
            }

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
            blogPageData.items = _blogRepository.getBlogPage(sql.ToString(), parameters);


            return blogPageData;
        }

        // random blog
        public List<BlogRandom> GetRandomBlog(BlogRandomFilter blogRF)
        {
            StringBuilder filter = new StringBuilder();
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            filter.Append(" limit @limit");
            parameters.Add(new MySqlParameter("@limit", blogRF.limit));
            return _blogRepository.GetRandomBlog(filter.ToString(), parameters);
        }


        // chi tiết blog
        public BlogDetail? GetBlogDetail(int id)
        {
            return _blogRepository.GetBlogDetail(id);
        }


        // thêm Blog
        public int AddBlog(BlogAdd blogAdd, int uid)
        {
            return _blogRepository.AddBlog(blogAdd, uid);
        }


        // kiểm tra tác giả Blog
        public bool CheckBlogBelongToUser(int blogId, int uid)
        {
            return _blogRepository.CheckBlogBelongToUser(blogId, uid);
        }

        // cập nhật Blog
        public BlogAdd? UpdateBlog(BlogAdd blogAdd, int id)
        {
            return _blogRepository.UpdateBlog(blogAdd, id);
        }

        // xóa Blog
        public void DeleteBlog(int id)
        {
            _blogRepository.DeleteBlog(id);
        }

        //// cập nhật trạng thái Blog
        //public int updateStatus(int blogID, Status status)
        //{
        //    MySqlParameter[] parameters = new MySqlParameter[]
        //    {
        //        new MySqlParameter("@blogID", blogID),
        //        new MySqlParameter("@status", status)
        //    };
        //    return _blogRepository.updateStatus(parameters);
        //}
        
        
        
        
        //// lấy blog list dành cho admin
        //public List<BlogList> GetBlogList(BlogListAdminFilter blogLAF)
        //{
        //    StringBuilder filter = new StringBuilder();
        //    List<MySqlParameter> parameters = new List<MySqlParameter>();

        //    if (!blogLAF.search.IsNullOrEmpty())
        //    {
        //        filter.Append(" where title = @title");
        //        parameters.Add(new MySqlParameter("@title", blogLAF.search));
        //        if (!blogLAF.type.IsNullOrEmpty())
        //        {
        //            filter.Append(" and type = @type");
        //            parameters.Add(new MySqlParameter("@type", blogLAF.type));

        //            if (!blogLAF.status.ToString().IsNullOrEmpty())
        //            {
        //                filter.Append(" and status = @status");
        //                parameters.Add(new MySqlParameter("@type", blogLAF.status));
        //            }
        //        }
        //        else
        //        {
        //            if (!blogLAF.status.ToString().IsNullOrEmpty())
        //            {
        //                filter.Append(" and status = @status");
        //                parameters.Add(new MySqlParameter("@type", blogLAF.status));
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (!blogLAF.type.IsNullOrEmpty())
        //        {
        //            filter.Append(" where type = @type");
        //            parameters.Add(new MySqlParameter("@type", blogLAF.type));

        //            if (!blogLAF.status.ToString().IsNullOrEmpty())
        //            {
        //                filter.Append(" and status = @status");
        //                parameters.Add(new MySqlParameter("@type", blogLAF.status));
        //            }
        //        }
        //        else
        //        {
        //            if (!blogLAF.status.ToString().IsNullOrEmpty())
        //            {
        //                filter.Append(" where status = @status");
        //                parameters.Add(new MySqlParameter("@type", blogLAF.status));
        //            }
        //        }
        //    }

        //    if (!blogLAF.sortBy.IsNullOrEmpty())
        //    {
        //        filter.Append(" order by @sortBy");
        //        parameters.Add(new MySqlParameter("@sortBy", blogLAF.sortBy));
        //    }
        //    else
        //    {
        //        filter.Append(" order by created_at");
        //    }

        //    if (!blogLAF.sortType.IsNullOrEmpty())
        //    {
        //        filter.Append(" @sortType");
        //        parameters.Add(new MySqlParameter("@sortType", blogLAF.sortType));
        //    }
        //    else
        //    {
        //        filter.Append(" desc");
        //    }

        //    if (blogLAF.limit != 12)
        //    {
        //        filter.Append(" limit @limit");
        //        parameters.Add(new MySqlParameter("@limit", blogLAF.limit));
        //    }
        //    else
        //    {
        //        filter.Append(" limit 12");
        //    }

        //    if (blogLAF.page != 1)
        //    {
        //        filter.Append(" offset @offset");
        //        if (blogLAF.limit == 12)
        //        {
        //            parameters.Add(new MySqlParameter("@offset", (blogLAF.page - 1) * 12));
        //        }
        //        else
        //        {
        //            parameters.Add(new MySqlParameter("@offset", (blogLAF.page - 1) * blogLAF.limit));
        //        }
        //    }
        //    else
        //    {
        //        filter.Append(" offset 0");
        //    }
        //    return _blogRepository.getBlogList(filter.ToString(), parameters);

        //}
        
    }
}
