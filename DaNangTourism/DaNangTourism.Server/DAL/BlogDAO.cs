using DaNangTourism.Server.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR.Protocol;
using MySqlConnector;
using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;

namespace DaNangTourism.Server.DAL
{
    public class BlogDAO
    {
        string url = "server=127.0.0.1;database=pbl3;uid=root;password=";
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
            try
            {
                string sql = "Select blog_id, type, title, image, user_id, created_at from blogs order by views desc limit 5";
                List<BlogHome> blogHomes = new List<BlogHome>();
                List<int> uid = new List<int>();
                using(MySqlConnection con = new MySqlConnection(url))
                {
                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            BlogHome blogHome = new BlogHome(reader);
                            uid.Add(reader.GetInt32("user_id"));
                            blogHomes.Add(blogHome);
                        }
                        reader.Close();
                        con.Close();
                    }
                }
                for(int i = 0; i < uid.Count; i++)
                {
                    blogHomes[i].authorName = AccountDAO.Instance.GetAccountById(uid[i]).Name;
                }
                return blogHomes;
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Error: " + ex.Message);
                return null; 
            }
        }
        // lấy blog theo page
        public List<BlogPage> getBlogPage(string filter, List<MySqlParameter> parameters)
        {
            try
            {
                List<BlogPage> blogPages = new List<BlogPage>();
                string sql = "Select blog_id, title, image, type, user_id, created_at, views, introduction " +
                             "from blogs" + filter;
                List<int> uid = new List<int>();

                dao.OpenConnection();
                MySqlDataReader reader = dao.ExecuteQuery(sql, parameters.ToArray());
                while (reader.Read())
                {
                    uid.Add(reader.GetInt32("user_id"));
                    blogPages.Add(new BlogPage(reader));
                }
                reader.Close();
                dao.CloseConnection();

                for (int i = 0; i < uid.Count; i++)
                {
                    blogPages[i].author = new Author(AccountDAO.Instance.GetAccountById(uid[i]));
                }
                return blogPages;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }
        public List<BlogList> getBlogList(string filter , List<MySqlParameter> parameters)
        {
            try
            {
                List<BlogList> blogLists = new List<BlogList>();
                string sql = "Select blog_id, title, type, user_id, created_at, status " +
                             "from blogs" + filter;
                List<int> uid = new List<int>();

                dao.OpenConnection();
                MySqlDataReader reader = dao.ExecuteQuery(sql, parameters.ToArray());
                while (reader.Read())
                {
                    uid.Add(reader.GetInt32("user_id"));
                    blogLists.Add(new BlogList(reader));
                }
                reader.Close();
                dao.CloseConnection();

                for(int i = 0; i < uid.Count; i ++)
                {
                    blogLists[i].authorName = AccountDAO.Instance.GetAccountById(uid[i]).Name;
                }
                return blogLists;
            }
            catch(Exception ex) 
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }
        //lấy blog dựa vào id // để hiển thị bài blog
        public BlogDetail getBlogDetail(MySqlParameter[] parameters)
        {
            try
            {
                BlogDetail blogDetail = new BlogDetail();
                string sql = "Select blog_id, title, type, user_id, created_at, views, blog_view from blogs where blog_id = @id";
                int uid = 0;

                dao.OpenConnection();
                MySqlDataReader reader = dao.ExecuteQuery(sql, parameters);
                while (reader.Read()) 
                {
                    uid = reader.GetInt32("user_id");
                    blogDetail = new BlogDetail(reader);
                }
                reader.Close();
                dao.CloseConnection();

                blogDetail.author = new Author(AccountDAO.Instance.GetAccountById(uid));                
                return blogDetail;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }
        // tăng view
        public int increaseView(MySqlParameter[] parameters)
        {
            string sql = "update blogs set views = views + 1 where blog_id = @id";
            dao.OpenConnection();
            int result = dao.ExecuteNonQuery(sql, parameters);
            dao.CloseConnection();
            return result;
        }
        // lấy random blog
        public List<BlogRandom> getRandomBlog(string filter, List<MySqlParameter> parameters)
        {
            try
            {
                List<BlogRandom> blogRandoms = new List<BlogRandom>();
                string sql = "Select blog_id, title, type, image, user_id, created_at from blogs " +
                             "order by rand()" + filter;
                List<int> uid = new List<int>();

                dao.OpenConnection();
                MySqlDataReader reader = dao.ExecuteQuery(sql, parameters.ToArray());
                while (reader.Read())
                {
                    uid.Add(reader.GetInt32("user_id"));
                    blogRandoms.Add(new BlogRandom(reader));
                }
                reader.Close();
                dao.CloseConnection();

                for (int i = 0; i < uid.Count; i++)
                {
                    blogRandoms[i].authorName = AccountDAO.Instance.GetAccountById(uid[i]).Name;
                }
                return blogRandoms;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }
        // cập nhật trạng thái Blog
        public int updateStatus(MySqlParameter[] parameters)
        {
            string sql = "update blogs set status = @status where id = @blogID";
            dao.OpenConnection();
            int result =  DAO.Instance.ExecuteNonQuery(sql, parameters);
            dao.CloseConnection();
            return result;
        }
        // thêm blog
        public int addBlog(MySqlParameter[] parameters)
        {
            string sql = "insert into blogs values (@id, @uid, @title, @type, @image, @introduction, " +
                "@created_at, @content, @views, @status)";
            dao.OpenConnection();
            int result = dao.ExecuteNonQuery(sql, parameters);
            dao.CloseConnection();
            return result;
        }
        // sửa blog
        public int updateBlog(MySqlParameter[] parameters)
        {
            string sql = "update blogs set title = @title, type = @type, image = @image, " +
                "introduction = @introduction, content = @content where id = @blogID";
            dao.OpenConnection();
            int result = dao.ExecuteNonQuery(sql, parameters);
            dao.CloseConnection();
            return result;
        }

        // xóa blog
        public int deleteBlog(MySqlParameter[] parameters)
        {
            string sql = "delete from blogs where blog_id = @blogID";
            dao.OpenConnection();
            int result = dao.ExecuteNonQuery(sql, parameters);
            dao.CloseConnection();
            return result;
        }
        // kiểm tra tác giả bài viết
        public int checkBlogBelongToUser(MySqlParameter[] parameters)
        {
            string sql = "select * from blogs where id = @blogID and uid = @uid";

            dao.OpenConnection();
            MySqlDataReader reader = DAO.Instance.ExecuteQuery(sql, parameters);
            if(reader.HasRows)
            {
                reader.Close();
                dao.CloseConnection();
                return 1;
            }
            else
            {
                reader.Close();
                dao.CloseConnection();
                return 0;
            }
        }
    }
}
