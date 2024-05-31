using MySqlConnector;
using DaNangTourism.Server.Models;

namespace DaNangTourism.Server.DAL
{
    public interface IFavoriteDestinationRepository
    {
        int AddFavDes(int userId, int destinationId);
        int DeleteFavDes(int userId, int destinationId);
        int GetFavDesCountByDesId(int destinationId);
    }
    public class FavoriteDestinationRepository : IFavoriteDestinationRepository
    {
        private readonly string _connectionString;
        public FavoriteDestinationRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Add Favorite Destination
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="destinationId"></param>
        /// <returns></returns>
        public int AddFavDes(int userId, int destinationId)
        {
            string sql = "INSERT INTO FavoriteDestinations(UserId, DestinationId) values (@userId, @destinationId)";
            MySqlParameter[] parameters = 
            {
                new ("@userId", userId),
                new ("@destinationId", destinationId)
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
            string sql = "DELETE FROM FavoriteDestinations WHERE UserId = @userId AND DestinationId = @destinationId";
            MySqlParameter[] parameters = 
            {
                new ("@userId", userId),
                new ("@destinationId", destinationId)
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
            string sql = "SELECT COUNT(*) FROM FavoriteDestinations WHERE DestinationId = @destinationId";
            var parameter = new MySqlParameter("@destinationId", destinationId);
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add(parameter);
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }
    }
}
