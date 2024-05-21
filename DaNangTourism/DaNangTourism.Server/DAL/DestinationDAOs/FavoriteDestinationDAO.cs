using MySqlConnector;
using DaNangTourism.Server.Models;

namespace DaNangTourism.Server.DAL
{
    public class FavoriteDestinationDAO
    {
        private readonly DAO _dao;
        private static FavoriteDestinationDAO _instance;
        public static FavoriteDestinationDAO Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FavoriteDestinationDAO(DAO.Instance);
                }
                return _instance;
            }
        }
        private FavoriteDestinationDAO(DAO dao)
        {
            _dao = dao;
        }
        /// <summary>
        /// Get all Favorite Destination by User Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<int> GetFavDesIds(int userId)
        {
            List<int> favDestinationIds = new List<int>();
            string sql = "select * from FavoriteDestination where UserId = @userId";
            MySqlParameter parameter = new MySqlParameter("@userId", userId);
            _dao.OpenConnection();
            MySqlDataReader reader = _dao.ExecuteQuery(sql, parameter);
            while (reader.Read())
            {
                int id = reader.GetInt32("destination_id");
                favDestinationIds.Add(id);
            }
            _dao.CloseConnection();
            return favDestinationIds;
        }
        /// <summary>
        /// Add Favorite Destination
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="destinationId"></param>
        /// <returns></returns>
        public int AddFavDes(int userId, int destinationId)
        {
            string sql = "insert into FavoriteDestination(UserId, DestinationId) values (@userId, @destinationId)";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@userId", userId),
                new MySqlParameter("@destinationId", destinationId)
            };
            _dao.OpenConnection();
            int result = _dao.ExecuteNonQuery(sql, parameters);
            _dao.CloseConnection();
            return result;
        }
        /// <summary>
        /// Delete Favorite Destination
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="destinationId"></param>
        /// <returns></returns>
        public int DeleteFavDes(int userId, int destinationId)
        {
            string sql = "delete from FavoriteDestination where UserId = @userId and DestinationId = @destinationId";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@userId", userId),
                new MySqlParameter("@destinationId", destinationId)
            };
            _dao.OpenConnection();
            int result = _dao.ExecuteNonQuery(sql, parameters);
            _dao.CloseConnection();
            return result;
        }
        /// <summary>
        /// Get count of destination which is add to favourite destination
        /// </summary>
        /// <param name="destinationId"></param>
        /// <returns></returns>
        public int GetFavDesCountByDesId(int destinationId)
        {
            string sql = "Select count(*) from FavoriteDestination where DestinationId = @destinationId";
            MySqlParameter parameter = new MySqlParameter("@destinationId", destinationId);
            int? result;
            _dao.OpenConnection();
            result = (int?)DAO.Instance.ExecuteScalar(sql, parameter);
            _dao.CloseConnection();
            if (result == null) result = 0;
            return result.Value;
        }
    }
}
