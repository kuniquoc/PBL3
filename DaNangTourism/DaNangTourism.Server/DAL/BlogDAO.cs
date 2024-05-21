﻿using DaNangTourism.Server.Models;
using Microsoft.AspNetCore.SignalR.Protocol;
using MySqlConnector;
using System.Runtime.CompilerServices;

namespace DaNangTourism.Server.DAL
{
    public class BlogDAO
    {
        DAO dao = DAO.Instance;
        public static BlogDAO? _instance;
        public static BlogDAO Instance
        {
            get
            {
                if(_instance == null) _instance = new BlogDAO();
                return _instance;
            }
            private set { }
        }
        public List<BlogHome> get5MostView()
        {
            List<BlogHome> blogHomes = new List<BlogHome>();
            string sql = "Select id, title, image, uid, created_at " +
                         "from blogs order by views desc limit 5";
            dao.OpenConnection();
            MySqlDataReader reader = dao.ExecuteQuery(sql);
            while (reader.Read())
            {
                blogHomes.Add(new BlogHome(reader));
            }
            dao.CloseConnection();
            return blogHomes;
        }
        // lấy tất cả blog
        public List<BlogPage> getAllBlogPage(int page)
        {
            List<BlogPage> blogPages = new List<BlogPage>();
            string sql = "Select id, title, image, type, uid, created_at, views, introduction " +
                         "from blogs limit 5 offset @page";
            dao.OpenConnection();
            MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@page", (page - 1) * 5) }; 
            MySqlDataReader reader = dao.ExecuteQuery(sql, null);
            while (reader.Read())
            {
                blogPages.Add(new BlogPage(reader));
            }
            dao.CloseConnection();
            return blogPages;
        }
        //lấy danh sách blog theo title
        public List<BlogPage> getBlogByTitle(string title)
        {
            List<BlogPage > blogPages = new List<BlogPage>();
            string sql = "Select id, title, image, type, uid, created_at, views, introduction " +
                         "from blogs where title = @title";
            MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@title", title) };
            dao.OpenConnection();
            MySqlDataReader reader = dao.ExecuteQuery(sql, parameters);
            while(reader.Read())
            {
                blogPages.Add(new BlogPage(reader));
            }
            dao.CloseConnection();
            return blogPages;
        }

    //   // lấy danh sách blog theo content
    //   public List<Blog> GetBlogByContent(string? content)
    //   {
    //       if(string.IsNullOrEmpty(content)) return GetAllBlog();
    //       List<Blog> blogs = new List<Blog>();
    //       string sql = "Select * from blogs where content = @content";
    //       MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter ("@content", content) };
    //       dao.OpenConnection();
    //       MySqlDataReader reader = dao.ExecuteQuery(sql, parameters);
    //       while (reader.Read())
    //       {
    //           Blog blog = new Blog(reader);
    //           blogs.Add (blog);
    //       }
    //       dao.CloseConnection() ;
    //       return blogs;
    //   }
        //lấy blog dựa vào id // để hiển thị bài blog
        public BlogDetail getBlogDetail(int id)
        {
            string sql = "Select id, title, type, uid, created_at, views, content" +
                         " from blogs where id = @id";
            MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@id", id) };
            dao.OpenConnection() ;
            MySqlDataReader reader = dao.ExecuteQuery(sql, parameters);
            BlogDetail blogDetail = new BlogDetail();
            while (reader.Read())
            {
                blogDetail = new BlogDetail(reader);
            }
            dao.CloseConnection();
            return blogDetail;
        }
        public void increaseView(int id)
        {
            string sql = "update blogs set views = views + 1 where id = @id";
            MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@id", id) };
            dao.ExecuteNonQuery(sql, parameters);
        }
        public List<BlogRandom> getRandomBlog()
        {
            List<BlogRandom> blogRandoms = new List<BlogRandom>();
            string sql = "Select id, title, type, image, uid, created_at from blogs " +
                         "order by rand() limit 5";
            dao.OpenConnection();
            MySqlDataReader reader = dao.ExecuteQuery(sql);
            while(reader.Read())
            {
                blogRandoms.Add(new BlogRandom(reader));
            }
            dao.CloseConnection();
            return blogRandoms;
        }
    //    // thêm blog
    //    public int AddBlog(Blog blog)
    //    {
    //        string sql = "insert into blogs values (@blogID, @userID, @title, @content, @postTime, @blogView)";
    //        MySqlParameter[] parameters = new MySqlParameter[]
    //        {
    //            new MySqlParameter("@blogID", blog.BlogID),
    //            new MySqlParameter("@userID", blog.UserID),
    //            new MySqlParameter("@title", blog.Title),
    //            new MySqlParameter("@content", blog.Content),
    //            new MySqlParameter("@postTime", blog.PostTime),
    //            new MySqlParameter("@blogView", blog.BlogView),
    //        };
    //        return dao.ExecuteNonQuery(sql, parameters);
    //    }
    //    // sửa blog
    //    public int EditBlog(Blog blog)
    //    {
    //        string sql = "update blogs set title = @title, content = @content, post_time = @postTime, blog_view = @blogView";
    //        MySqlParameter[] parameters = new MySqlParameter[]
    //        {
    //            new MySqlParameter("@title", blog.Title),
    //            new MySqlParameter("@content", blog.Content),
    //            new MySqlParameter("@postTime", blog.PostTime),
    //            new MySqlParameter("@blogView", blog.BlogView),
    //        };
    //        return dao.ExecuteNonQuery(sql, parameters);
    //    }
    //    // xóa blog
    //    public int DeleteBlog(int blogID)
    //    {
    //        string sql = "delete from blogs where blog_id = @blogID";
    //        MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@blogID", blogID) };
    //        return dao.ExecuteNonQuery(sql, parameters);
    //    }
    }
}
