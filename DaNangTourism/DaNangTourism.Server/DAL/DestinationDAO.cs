using MySqlConnector;
using DaNangTourism.Server.Models;

namespace DaNangTourism.Server.DAL
{
    public class DestinationDAO
    {
        public Dictionary<int, Destination> GetAllDestinations()
        {
            DAO dao = new DAO();
            Dictionary<int, Destination> destinations = new Dictionary<int, Destination>();
            string sql = "Select * from destinations";
            MySqlDataReader reader = dao.ExecuteQuery(sql, null);
            while (reader.Read())
            {
                Destination destination = new Destination(reader);
                destinations.Add(destination.Id, destination);
            }
            return destinations;
        }
        public List<Destination> GetDescendingDestination()
        {
            DAO dao = new DAO();
            List<Destination> destinations = new List<Destination>();
            // Lấy danh sách các destination theo thứ tự rating giảm dần từ db;
            string sql = "Select * from destinations order by rating desc;";
            MySqlDataReader reader = dao.ExecuteQuery(sql, null);
            while (reader.Read())
            {
                Destination destination = new Destination(reader);
                destinations.Add(destination);
            }
            return destinations;
        }
        public Destination? GetDestinationsById(int id)
        {
            DAO dao = new DAO();
            string sql = "Select * from destinations where destination_id = @id";
            MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@id", id) };
            MySqlDataReader reader = dao.ExecuteQuery(sql, parameters);
            if (reader.Read())
            {
                Destination destination = new Destination(reader);
                return destination;
            }
            else return null;
        }

        public Dictionary<int, Destination> GetDestinationsByIds(List<int> ids)
        {
            DAO dao = new DAO();
            string sql = "Select * from destinations where destination_id in (@id0" ;
            MySqlParameter[] parameters = new MySqlParameter[ids.Count];
            parameters[0] = new MySqlParameter("id0", ids[0]);
            for (int i =  1; i < ids.Count; i++)
            {
                string parameter = "@id" + i;
                sql += ", " + parameter;
                parameters[i] = new MySqlParameter(parameter, ids[i]);
            }
            sql += ");" ;

            Dictionary<int, Destination> destinations = new Dictionary<int, Destination>();
            MySqlDataReader reader = dao.ExecuteQuery(sql, parameters);
            while (reader.Read())
            {
                Destination destination = new Destination(reader);
                destinations.Add(destination.Id, destination);
            }
            return destinations;
        }
        public int AddDestination(Destination destination)
        {
            DAO dao = new DAO();
            string sql = "Insert into destinations(destination_name, destination_address, open_time, close_time, open_day, destination_html, destination_image_url, rating)" +
                "values (@destination_name, @destination_address, @open_time, @close_time, @open_day, @destination_html, @destination_image_url, @rating)";
            MySqlParameter[] parameters = new MySqlParameter[8];
            parameters[0] = new MySqlParameter("@destination_name", destination.Name);
            parameters[1] = new MySqlParameter("@destination_address", destination.Address);
            parameters[2] = new MySqlParameter("@open_time", destination.OpenTime.ToString("HH:mm:ss"));
            parameters[3] = new MySqlParameter("@close_time", destination.CloseTime.ToString("HH:mm:ss"));
            parameters[4] = new MySqlParameter("@open_day", destination.OpenDay);
            parameters[5] = new MySqlParameter("@destination_html", destination.HtmlText);
            parameters[6] = new MySqlParameter("@destination_image_url", string.Join(';', destination.ImgURL));
            parameters[7] = new MySqlParameter("@rating", destination.Rating);

            return dao.ExecuteNonQuery(sql, parameters);
        }
        public int UpdateDestination(Destination destination)
        {
            DAO dao = new DAO();
            string sql = "Update destinations set destination_name = @destination_name, destination_address = @destination_address, " +
                "open_time = @open_time, close_time = @close_time, open_day = @open_day, destination_html = @destination_html, " +
                "destination_image_url = @destination_image_url, rating = @rating where destination_id = @destination_id";
            MySqlParameter[] parameters = new MySqlParameter[9];
            parameters[0] = new MySqlParameter("@destination_name", destination.Name);
            parameters[1] = new MySqlParameter("@destination_address", destination.Address);
            parameters[2] = new MySqlParameter("@open_time", destination.OpenTime);
            parameters[3] = new MySqlParameter("@close_time", destination.CloseTime);
            parameters[4] = new MySqlParameter("@open_day", destination.OpenDay);
            parameters[5] = new MySqlParameter("@destination_html", destination.HtmlText);
            parameters[6] = new MySqlParameter("@destination_image_url", string.Join(';',destination.ImgURL));
            parameters[7] = new MySqlParameter("@rating", destination.Rating);
            parameters[8] = new MySqlParameter("@destination_id", destination.Id);

            return dao.ExecuteNonQuery(sql, parameters);
        }
        public int DeleteDestination(int id)
        {
            DAO dao = new DAO();
            string sql = "Delete from destinations where destination_id = @id";
            MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@id", id) };
            return dao.ExecuteNonQuery(sql, parameters);
        }
    }
}
