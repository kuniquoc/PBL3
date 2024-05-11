using DaNangTourism.Server.Models;
using Microsoft.AspNetCore.SignalR.Protocol;
using MySqlConnector;
using System.Runtime.CompilerServices;

namespace DaNangTourism.Server.DAL
{
    public class BlogDAO
    {
        // lấy tất cả blog
        public Dictionary<int, Blog> GetAllBlog()
        {
            DAO dao = new DAO();
            Dictionary<int, Blog> blogs = new Dictionary<int, Blog>();
            string sql = "Select * from blogs";
            MySqlDataReader reader = dao.ExecuteQuery(sql, null);
            while (reader.Read())
            {
                Blog blog = new Blog(reader);
                blogs.Add(blog.BlogID, blog);
            }
            return blogs;
        }
        // lấy danh sách blog theo content
        public Dictionary<int, Blog> GetBlogByContent(string? content)
        {
            if(string.IsNullOrEmpty(content)) return GetAllBlog();
            DAO dao = new DAO();
            Dictionary<int, Blog> blogs = new Dictionary<int, Blog>();
            string sql = "Select * from blogs where content = @content";
            MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter ("@content", content) };
            MySqlDataReader reader = dao.ExecuteQuery(sql, parameters);
            while (reader.Read())
            {
                Blog blog = new Blog(reader);
                blogs.Add (blog.BlogID, blog);
            }
            return blogs;
        }
        //lấy blog dựa vào id // để hiển thị bài blog
        public Blog GetBlogByBlogID(int blogID)
        {
            DAO dao = new DAO();
            string sql = "Select * from blogs where blog_id = @blogID";
            MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@blogID", blogID) };
            MySqlDataReader reader = dao.ExecuteQuery(sql, parameters);
            Blog blog = new Blog();
            while (reader.Read())
            {
                blog = new Blog(reader);
            }
            return blog;
        }
        // thêm blog
        public int AddBlog(Blog blog)
        {
            DAO dao = new DAO();
            string sql = "insert into blogs values (@blogID, @userID, @title, @content, @postTime, @blogView)";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@blogID", blog.BlogID),
                new MySqlParameter("@userID", blog.UserID),
                new MySqlParameter("@title", blog.Title),
                new MySqlParameter("@content", blog.Content),
                new MySqlParameter("@postTime", blog.PostTime),
                new MySqlParameter("@content", blog.Content),
            };
            return dao.ExecuteNonQuery(sql, parameters);
        }
        // sửa blog
        public int EditBlog(Blog blog)
        {
            DAO dao = new DAO();
            string sql = "update blogs set title = @title, content = @content, post_time = @postTime, blog_view = @blogView";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@title", blog.Title),
                new MySqlParameter("@content", blog.Content),
                new MySqlParameter("@postTime", blog.PostTime),
                new MySqlParameter("@blogView", blog.BlogView),
            };
            return dao.ExecuteNonQuery(sql, parameters);
        }
        // xóa blog
        public int DeleteBlog(int blogID)
        {
            DAO dao = new DAO();
            string sql = "delete from blogs where blog_id = @blogID";
            MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@blogID", blogID) };
            return dao.ExecuteNonQuery(sql, parameters);
        }
    }
}
