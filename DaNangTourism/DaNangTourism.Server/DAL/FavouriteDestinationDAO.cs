using MySqlConnector;
using DaNangTourism.Server.Models;

namespace DaNangTourism.Server.DAL
{
    public class FavouriteDestinationDAO
    {
        private readonly DAO _dao;
        private static FavouriteDestinationDAO _instance;
        public static FavouriteDestinationDAO Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FavouriteDestinationDAO(DAO.Instance);
                }
                return _instance;
            }
        }
        private FavouriteDestinationDAO(DAO dao)
        {
            _dao = dao;
        }
        public List<int> GetFavDesIds(int userId)
        {
            List<int> favDestinationIds = new List<int>();
            string sql = "select * from favourite_destinations where user_id = @userId";
            MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@userId", userId) };
            _dao.OpenConnection();
            MySqlDataReader reader = _dao.ExecuteQuery(sql, parameters);
            while (reader.Read())
            {
                int id = reader.GetInt32("destination_id");
                favDestinationIds.Add(id);
            }
            _dao.CloseConnection();
            return favDestinationIds;
        }
        public int AddFavDes(int userId, int destinationId)
        {
            string sql = "insert into favourite_destinations(user_id, destination_id) values (@userId, @destinationId)";
            MySqlParameter[] parameters = new MySqlParameter[2];
            parameters[0] = new MySqlParameter("@userId", userId);
            parameters[1] = new MySqlParameter("@destinationId", destinationId);
            _dao.OpenConnection();
            int result = _dao.ExecuteNonQuery(sql, parameters);
            _dao.CloseConnection();
            return result;
        }
        public int DeleteFavDes(int userId, int destinationId)
        {
            string sql = "delete from favourite_destinations where user_id = @userId and destination_id = @destinationId";
            MySqlParameter[] parameters = new MySqlParameter[2];
            parameters[0] = new MySqlParameter("@userId", userId);
            parameters[1] = new MySqlParameter("@destinationId", destinationId);
            _dao.OpenConnection();
            int result = _dao.ExecuteNonQuery(sql, parameters);
            _dao.CloseConnection();
            return result;
        }
    }
}
