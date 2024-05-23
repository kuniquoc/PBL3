using DaNangTourism.Server.Models;
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
        // lấy 5 blog được view cao nhất
        public List<BlogHome> get5MostView()
        {
            List<BlogHome> blogHomes = new List<BlogHome>();
            string sql = "Select blog_id, title, image, user_id, created_at " +
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
        // lấy blog theo page
        public List<BlogPage> getBlogPage(string filter, List<MySqlParameter> parameters)
        {
            List<BlogPage> blogPages = new List<BlogPage>();
            string sql = "Select blog_id, title, image, type, user_id, created_at, views, introduction " +
                         "from blogs" + filter;
            dao.OpenConnection();
            MySqlDataReader reader = dao.ExecuteQuery(sql, parameters.ToArray());
            while (reader.Read())
            {
                blogPages.Add(new BlogPage(reader));
            }
            dao.CloseConnection();
            return blogPages;
        }
        public List<BlogList> getBlogList(string filter , List<MySqlParameter> parameters)
        {
            List<BlogList> blogLists = new List<BlogList>();
            string sql = "Select blog_id, title, type, user_id, created_at, status " +
                         "from blogs" + filter;
            dao.OpenConnection();
            MySqlDataReader reader = dao.ExecuteQuery(sql, parameters.ToArray());
            while (reader.Read())
            {
                blogLists.Add(new BlogList(reader));
            }
            dao.CloseConnection();
            return blogLists;
        }
        //lấy blog dựa vào id // để hiển thị bài blog
        public BlogDetail getBlogDetail(MySqlParameter[] parameters)
        {
            string sql = "Select blog_id, title, type, user_id, created_at, views, blog_view" +
                         " from blogs where id = @id";
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
        // tăng view
        public int increaseView(MySqlParameter[] parameters)
        {
            string sql = "update blogs set views = views + 1 where blog_id = @id";            
            return dao.ExecuteNonQuery(sql, parameters);
        }
        // lấy random blog
        public List<BlogRandom> getRandomBlog(string filter, List<MySqlParameter> parameters)
        {
            List<BlogRandom> blogRandoms = new List<BlogRandom>();
            string sql = "Select blog_id, title, type, image, user_id, created_at from blogs " +
                         "order by rand()" + filter;
            dao.OpenConnection();
            MySqlDataReader reader = dao.ExecuteQuery(sql, parameters.ToArray());
            while(reader.Read())
            {
                blogRandoms.Add(new BlogRandom(reader));
            }
            dao.CloseConnection();
            return blogRandoms;
        }
        // cập nhật trạng thái Blog
        public int updateStatus(MySqlParameter[] parameters)
        {
            string sql = "update blogs set status = @status where id = @blogID";
            return DAO.Instance.ExecuteNonQuery(sql, parameters);
        }
        // thêm blog
        public int addBlog(MySqlParameter[] parameters)
        {
            string sql = "insert into blogs values (@id, @uid, @title, @type, @image, @introduction, " +
                "@created_at, @content, @views, @status)";
            return dao.ExecuteNonQuery(sql, parameters);
        }
        // sửa blog
        public int updateBlog(MySqlParameter[] parameters)
        {
            string sql = "update blogs set title = @title, type = @type, image = @image, " +
                "introduction = @introduction, content = @content where id = @blogID";            
            return dao.ExecuteNonQuery(sql, parameters);
        }

        // xóa blog
        public int deleteBlog(MySqlParameter[] parameters)
        {
            string sql = "delete from blogs where blog_id = @blogID";
            return dao.ExecuteNonQuery(sql, parameters);
        }
        // kiểm tra tác giả bài viết
        public int checkBlogBelongToUser(MySqlParameter[] parameters)
        {
            string sql = "select * from blogs where id = @blogID and uid = @uid";
            MySqlDataReader reader = DAO.Instance.ExecuteQuery(sql, parameters);
            if(reader.HasRows)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
