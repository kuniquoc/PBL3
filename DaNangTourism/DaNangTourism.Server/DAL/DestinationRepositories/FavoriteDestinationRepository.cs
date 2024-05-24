using MySqlConnector;
using DaNangTourism.Server.Models;

namespace DaNangTourism.Server.DAL
{
    public interface IFavoriteDestinationRepository
    {
        IEnumerable<int> GetFavDesIds(int userId);
        int AddFavDes(int userId, int destinationId);
        int DeleteFavDes(int userId, int destinationId);
        int GetFavDesCountByDesId(int destinationId);
    }
    public class FavoriteDestinationRepository : IFavoriteDestinationRepository
    {
        private string _connectionString;
        public FavoriteDestinationRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Get all Favorite Destination by User Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<int> GetFavDesIds(int userId)
        {
            string sql = "select * from FavoriteDestinations where UserId = @userId";
            MySqlParameter parameter = new MySqlParameter("@userId", userId);
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    var favDestinationIds = new List<int>();
                    command.Parameters.Add(parameter);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(2);
                            favDestinationIds.Add(id);
                        }
                    }
                    return favDestinationIds;
                }
            }
        }
        /// <summary>
        /// Add Favorite Destination
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="destinationId"></param>
        /// <returns></returns>
        public int AddFavDes(int userId, int destinationId)
        {
            string sql = "insert into FavoriteDestinations(UserId, DestinationId) values (@userId, @destinationId)";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@userId", userId),
                new MySqlParameter("@destinationId", destinationId)
            };
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddRange(parameters);
                    return command.ExecuteNonQuery();
                }
            }
        }
        /// <summary>
        /// Delete Favorite Destination
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="destinationId"></param>
        /// <returns></returns>
        public int DeleteFavDes(int userId, int destinationId)
        {
            string sql = "delete from FavoriteDestinations where UserId = @userId and DestinationId = @destinationId";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@userId", userId),
                new MySqlParameter("@destinationId", destinationId)
            };
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddRange(parameters);
                    return command.ExecuteNonQuery();
                }
            }
        }
        /// <summary>
        /// Get count of destination which is add to favourite destination
        /// </summary>
        /// <param name="destinationId"></param>
        /// <returns></returns>
        public int GetFavDesCountByDesId(int destinationId)
        {
            string sql = "Select count(*) from FavoriteDestinations where DestinationId = @destinationId";
            MySqlParameter parameter = new MySqlParameter("@destinationId", destinationId);
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add(parameter);
                    return (int)command.ExecuteScalar();
                }
            }
        }
    }
}
