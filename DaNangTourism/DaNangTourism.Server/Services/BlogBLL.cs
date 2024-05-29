using DaNangTourism.Server.DAL;
using DaNangTourism.Server.Models;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace DaNangTourism.Server.Services
{
    public class BlogBLL
    {
        //private static BlogBLL? _instance;
        //public static BlogBLL Instance
        //{
        //    get 
        //    {
        //        if( _instance == null ) _instance = new BlogBLL();
        //        return _instance;
        //    }
        //    private set { }
        //}
        //// tăng view
        //public int increaseView(int id)
        //{
        //    MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@id", id) };
        //    return BlogDAO.Instance.increaseView(parameters);
        //}
        //// 5 bài có lượt xem cao nhất
        //public List<BlogHome> get5MostView()
        //{
        //    return BlogDAO.Instance.get5MostView();
        //}
        //// chi tiết blog
        //public BlogDetail getBlogDetail(int id)
        //{
        //    MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@id", id)};
        //    return BlogDAO.Instance.getBlogDetail(parameters);
        //}
        //// random blog
        //public List<BlogRandom> getRandomBlog(BlogRandomFilter blogRF)
        //{
        //    StringBuilder filter = new StringBuilder();
        //    List<MySqlParameter> parameters = new List<MySqlParameter>();   
        //    if (blogRF.limit != 5)
        //    {
        //        filter.Append(" limit @limit");
        //        parameters.Add(new MySqlParameter("@limit", blogRF.limit));
        //    }
        //    else
        //    {
        //        filter.Append(" limit 5");
        //    }
        //    return BlogDAO.Instance.getRandomBlog(filter.ToString(), parameters);
        //}
        //// cập nhật trạng thái Blog
        //public int updateStatus(int blogID, Status status) 
        //{
        //    MySqlParameter[] parameters = new MySqlParameter[]
        //    {
        //        new MySqlParameter("@blogID", blogID),
        //        new MySqlParameter("@status", status)
        //    };
        //    return BlogDAO.Instance.updateStatus(parameters);
        //}
        //// cập nhật Blog
        //public int updateBlog(BlogAdd blogAdd, int id)
        //{
        //    MySqlParameter[] parameters = new MySqlParameter[]
        //    {
        //        new MySqlParameter("@title", blogAdd.title),
        //        new MySqlParameter("@type", blogAdd.type),
        //        new MySqlParameter("@image", blogAdd.image),
        //        new MySqlParameter("@introduction", blogAdd.introduction),
        //        new MySqlParameter("@content", blogAdd.content),
        //        new MySqlParameter("@blogID", id),
        //    };
        //    return BlogDAO.Instance.updateBlog(parameters);
        //}
        //// kiểm tra tác giả Blog
        //public int checkBlogBelongToUser(int blogId, int uid)
        //{
        //    MySqlParameter[] parameters = new MySqlParameter[]
        //    {
        //        new MySqlParameter("@blogID", blogId),
        //        new MySqlParameter("@uid", uid)
        //    };
        //    return BlogDAO.Instance.checkBlogBelongToUser(parameters);
        //}
        //// xóa Blog
        //public int deleteBlog(int id)
        //{
        //    MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@blogID", id) };
        //    return BlogDAO.Instance.deleteBlog(parameters);
        //}
        //// thêm Blog
        //public int addBlog(BlogAdd blogAdd, int uid)
        //{
        //    MySqlParameter[] parameters = new MySqlParameter[]
        //    {
        //        new MySqlParameter("@id",null),
        //        new MySqlParameter("@uid", uid),
        //        new MySqlParameter("@title", blogAdd.title),
        //        new MySqlParameter("@type", blogAdd.type),
        //        new MySqlParameter("@image", blogAdd.image),
        //        new MySqlParameter("@introduction", blogAdd.introduction),
        //        new MySqlParameter("@created_at", DateTime.Now),
        //        new MySqlParameter("@content", blogAdd.content),
        //        new MySqlParameter("@views", 0),
        //        new MySqlParameter("@status", Status.pending),
        //    };
        //    return BlogDAO.Instance.addBlog(parameters);
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
        //        if(!blogLAF.type.IsNullOrEmpty())
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
        //            if(!blogLAF.status.ToString().IsNullOrEmpty())
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
        //    return BlogDAO.Instance.getBlogList(filter.ToString(), parameters);
            
        //}
        //// lấy Blog đặt vào giao diện chính
        //public List<BlogPage> getBlogPage(BlogPageFilter blogPF)
        //{
        //    StringBuilder filter = new StringBuilder();
        //    List<MySqlParameter> parameters = new List<MySqlParameter>();

        //    if (!blogPF.search.IsNullOrEmpty())
        //    {
        //        filter.Append(" where title like @title");
        //        parameters.Add(new MySqlParameter("@title", "'%" + blogPF.search + "%'"));
        //    }

        //    if (!blogPF.sortBy.IsNullOrEmpty())
        //    {
        //        filter.Append(" order by @sortBy");
        //        parameters.Add(new MySqlParameter("@sortBy", blogPF.sortBy));
        //    }
        //    else
        //    {
        //        filter.Append(" order by created_at");
        //    }

        //    if (!blogPF.sortType.IsNullOrEmpty())
        //    {
        //        filter.Append(" @sortType");
        //        parameters.Add(new MySqlParameter("@sortType", blogPF.sortType));
        //    }
        //    else
        //    {
        //        filter.Append(" desc");
        //    }

        //    if (blogPF.limit != 5)
        //    {
        //        filter.Append(" limit @limit");
        //        parameters.Add(new MySqlParameter("@limit", blogPF.limit));
        //    }
        //    else
        //    {
        //        filter.Append(" limit 5");
        //    }

        //    if (blogPF.page != 1)
        //    {
        //        filter.Append(" offset @offset");
        //        if (blogPF.limit == 5)
        //        {                        
        //            parameters.Add(new MySqlParameter("@offset", (blogPF.page - 1) * 5));
        //        }
        //        else
        //        {
        //            parameters.Add(new MySqlParameter("@offset", (blogPF.page - 1) * blogPF.limit));
        //        }
        //    }
        //    else
        //    {
        //        filter.Append(" offset 0");
        //    }
        //    return BlogDAO.Instance.getBlogPage(filter.ToString(), parameters);
        //}
    }
}
