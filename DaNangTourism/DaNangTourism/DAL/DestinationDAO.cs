using MySqlConnector;
using DaNangTourism.Models;

namespace DaNangTourism.DAL
{
    public class DestinationDAO
    {
        public Dictionary<int, Destination> GetAllDestination()
        {
            DAO dao = new DAO();
            Dictionary<int, Destination> destinations = new Dictionary<int, Destination>();
            string sql = "Select * from destinations";
            MySqlDataReader reader = dao.ExecuteQuery(sql, null);
            while (reader.Read())
            {
                Destination destination = new Destination();
                destination.Id = reader.GetInt32("destination_id");
                destination.Name = reader.GetString("destination_name");
                destination.Address = reader.GetString("destination_address");
                destination.OpenTime = TimeOnly.Parse(reader.GetString("open_time"));
                destination.CloseTime = TimeOnly.Parse(reader.GetString("close_time"));
                destination.OpenDay = Enum.Parse<DayOfWeek>(reader.GetString("destination_address"));
                destination.HtmlText = reader.GetString("destination_address");
                destination.ImgURL = reader.GetString("destination_address").Split(';');
                destination.Rating = reader.GetFloat("rating");
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
                Destination destination = new Destination();
                destination.Id = reader.GetInt32("destination_id");
                destination.Name = reader.GetString("destination_name");
                destination.Address = reader.GetString("destination_address");
                destination.OpenTime = TimeOnly.Parse(reader.GetString("open_time"));
                destination.CloseTime = TimeOnly.Parse(reader.GetString("close_time"));
                destination.OpenDay = Enum.Parse<DayOfWeek>(reader.GetString("destination_address"));
                destination.HtmlText = reader.GetString("destination_address");
                destination.ImgURL = reader.GetString("destination_address").Split(';');
                destination.Rating = reader.GetFloat("rating");
                destinations.Add(destination);
            }
            return destinations;
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
                Destination destination = new Destination();
                destination.Id = reader.GetInt32("destination_id");
                destination.Name = reader.GetString("destination_name");
                destination.Address = reader.GetString("destination_address");
                destination.OpenTime = TimeOnly.Parse(reader.GetString("open_time"));
                destination.CloseTime = TimeOnly.Parse(reader.GetString("close_time"));
                destination.OpenDay = Enum.Parse<DayOfWeek>(reader.GetString("destination_address"));
                destination.HtmlText = reader.GetString("destination_address");
                destination.ImgURL = reader.GetString("destination_address").Split(';');
                destination.Rating = reader.GetFloat("rating");
                destinations.Add(destination.Id, destination);
            }
            return destinations;
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
