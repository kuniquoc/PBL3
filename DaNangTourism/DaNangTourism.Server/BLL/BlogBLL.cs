using DaNangTourism.Server.DAL;
using DaNangTourism.Server.Models;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;

namespace DaNangTourism.Server.BLL
{
    public class BlogBLL
    {
        private static BlogBLL? _instance;
        public static BlogBLL Instance
        {
            get 
            {
                if( _instance == null ) _instance = new BlogBLL();
                return _instance;
            }
            private set { }
        }
        public List<BlogHome> get5MostView()
        {
            return BlogDAO.Instance.get5MostView();
        }
        public BlogDetail getBlogDetail(int id)
        {
            MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@id", id)};
            return BlogDAO.Instance.getBlogDetail(parameters);
        }
        public List<BlogRandom> getRandomBlog(IQueryCollection query)
        {
            string filter = "";
            List<MySqlParameter> parameters = new List<MySqlParameter>();   
            if (!query["limit"].IsNullOrEmpty())
            {
                filter += " limit @limit";
                parameters.Add(new MySqlParameter("@limit", Convert.ToInt32(query["limit"])));
            }
            else
            {
                filter += " limit 5";
            }
            return BlogDAO.Instance.getRandomBlog(filter, parameters);
        }
        public int addBlog(BlogAdd blogAdd, int uid)
        {
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@id",null),
                new MySqlParameter("@uid", uid),
                new MySqlParameter("@title", blogAdd.title),
                new MySqlParameter("@type", blogAdd.type),
                new MySqlParameter("@image", blogAdd.image),
                new MySqlParameter("@introduction", blogAdd.introduction),
                new MySqlParameter("@created_at", DateTime.Now),
                new MySqlParameter("@content", blogAdd.content),
                new MySqlParameter("@views", 0),
                new MySqlParameter("@status", Status.pending),
            };
            return BlogDAO.Instance.addBlog(parameters);
        }
        public List<BlogPage> getBlogPage(IQueryCollection query)
        {
            string filter = "";
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            if (!query["page"].IsNullOrEmpty() || !query["limit"].IsNullOrEmpty()
             || !query["search"].IsNullOrEmpty() || !query["sortBy"].IsNullOrEmpty()
             || !query["sortType"].IsNullOrEmpty())
            {
                if (!query["search"].IsNullOrEmpty())
                {
                    filter += " where title like @title";
                    parameters.Add(new MySqlParameter("@title", "'%" + query["search"] + "%'"));
                }

                if (!query["sortBy"].IsNullOrEmpty())
                {
                    filter += " order by @sortBy";
                    parameters.Add(new MySqlParameter("@sortBy", query["sortBy"]));
                }
                else
                {
                    filter += " order by created_at";
                }

                if (!query["sortType"].IsNullOrEmpty())
                {
                    filter += " @sortType";
                    parameters.Add(new MySqlParameter("@sortType", query["sortType"]));
                }
                else
                {
                    filter += " desc";
                }

                if (!query["limit"].IsNullOrEmpty())
                {
                    filter += " limit @limit";
                    parameters.Add(new MySqlParameter("@limit", Convert.ToInt32(query["limit"])));
                }
                else
                {
                    filter += " limit 5";
                }

                if (!query["page"].IsNullOrEmpty())
                {
                    if (query["limit"].IsNullOrEmpty())
                    {
                        filter += " offset @offset";
                        parameters.Add(new MySqlParameter("@offset", (Convert.ToInt32(query["page"]) - 1) * 5));
                    }
                    else
                    {
                        int page = (Convert.ToInt32(query["page"]) - 1) * Convert.ToInt32(query["limit"]);
                        filter += " offset @offset";
                        parameters.Add(new MySqlParameter("@offset", page));
                    }
                }
                else
                {
                    filter += " offset 0";
                }
                return BlogDAO.Instance.getBlogPage(filter, parameters);
            }
            else
            {
                filter += " order by created_at desc";
                return BlogDAO.Instance.getBlogPage(filter, parameters);
            }
        }
    }
}
