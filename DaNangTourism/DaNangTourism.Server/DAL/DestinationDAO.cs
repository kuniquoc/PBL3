using MySqlConnector;
using DaNangTourism.Server.Models;

namespace DaNangTourism.Server.DAL
{
    public class DestinationDAO
    {
        private readonly DAO _dao;
        private static DestinationDAO _instance;
        public static DestinationDAO Instance
        {
            get
            {
                _instance = new DestinationDAO(DAO.Instance);
                return _instance;
            }
            private set { }
        }
        private DestinationDAO(DAO dao)
        {
            _dao = dao;
        }
        public Dictionary<int, Destination> GetAllDestinations()
        {
            Dictionary<int, Destination> destinations = new Dictionary<int, Destination>();
            string sql = "Select * from destinations";
            _dao.OpenConnection();
            MySqlDataReader reader = _dao.ExecuteQuery(sql, null);
            while (reader.Read())
            {
                Destination destination = new Destination(reader);
                destinations.Add(destination.Id, destination);
            }
            _dao.CloseConnection();
            return destinations;
        }
        public List<Destination> GetDescendingDestination()
        {
            List<Destination> destinations = new List<Destination>();
            // Lấy danh sách các destination theo thứ tự rating giảm dần từ db;
            string sql = "Select * from destinations order by rating desc;";
            _dao.OpenConnection();
            MySqlDataReader reader = _dao.ExecuteQuery(sql, null);
            while (reader.Read())
            {
                Destination destination = new Destination(reader);
                destinations.Add(destination);
            }
            _dao.CloseConnection();
            return destinations;
        }
        public Destination? GetDestinationById(int id)
        {
            string sql = "Select * from destinations where destination_id = @id";
            MySqlParameter[] parameters = [new MySqlParameter("@id", id)];
            _dao.OpenConnection();
            MySqlDataReader reader = _dao.ExecuteQuery(sql, parameters);
            if (reader.Read())
            {
                Destination destination = new Destination(reader);
                _dao.CloseConnection();
                return destination;
            }
            _dao.CloseConnection();
            return null;
        }

        public Dictionary<int, Destination> GetDestinationsByIds(List<int> ids)
        {
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
            _dao.OpenConnection();
            MySqlDataReader reader = _dao.ExecuteQuery(sql, parameters);
            while (reader.Read())
            {
                Destination destination = new Destination(reader);
                destinations.Add(destination.Id, destination);
            }
            _dao.CloseConnection();
            return destinations;
        }
        public int AddDestination(Destination destination)
        {
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
            _dao.OpenConnection();
            int result = _dao.ExecuteNonQuery(sql, parameters);
            _dao.CloseConnection();
            return result;
        }
        public int UpdateDestination(Destination destination)
        {
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

            _dao.OpenConnection();
            int result = _dao.ExecuteNonQuery(sql, parameters);
            _dao.CloseConnection();
            return result;
        }
        public int DeleteDestination(int id)
        {
            string sql = "Delete from destinations where destination_id = @id";
            MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@id", id) };
            _dao.OpenConnection();
            int result = _dao.ExecuteNonQuery(sql, parameters);
            _dao.CloseConnection();
            return result;
        }
    }
}
