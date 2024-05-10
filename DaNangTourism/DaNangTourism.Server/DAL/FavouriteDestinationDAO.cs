using MySqlConnector;
using DaNangTourism.Server.Models;

namespace DaNangTourism.Server.DAL
{
    public class FavouriteDestinationDAO
    {
        public List<int> GetAllFavouriteDestination(int userId)
        {
            List<int> favDestinationIds = new List<int>();
            string sql = "select * from favourite_destinations where user_id = @userId";
            MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@userId", userId) };
            DAO dao = DAO.Instance;
            MySqlDataReader reader = dao.ExecuteQuery(sql, parameters);
            while (reader.Read())
            {
                int id = reader.GetInt32("destination_id");
                favDestinationIds.Add(id);
            }
            return favDestinationIds;
        }
    }
}
