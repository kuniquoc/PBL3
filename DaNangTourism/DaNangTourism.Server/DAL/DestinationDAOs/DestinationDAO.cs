using MySqlConnector;
using DaNangTourism.Server.Models;
using DaNangTourism.Server.Models.DestinationModels;

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
                if (_instance == null)
                    _instance = new DestinationDAO(DAO.Instance);
                return _instance;
            }
            private set { }
        }
        private DestinationDAO(DAO dao)
        {
            _dao = dao;
        }
        /// <summary>
        /// Get limit Destination to Home Page
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        public List<HomeDestination> GetNewestDestinations(int limit = 5)
        {
            List<HomeDestination> destinations = new List<HomeDestination>();
            string sql = "Select DestinationId, Name, Address, Images, Rating from Destinations order by Created_At desc limit @limit";
            MySqlParameter parameter = new MySqlParameter("@limit", limit);
            _dao.OpenConnection();
            MySqlDataReader reader = _dao.ExecuteQuery(sql, parameter);
            while (reader.Read())
            {
                HomeDestination destination = new HomeDestination(reader);
                destinations.Add(destination);
            }
            _dao.CloseConnection();
            return destinations;
        }
        /// <summary>
        /// Get list destination for destination page
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<ListDestination> GetListDestination(string filter = "")
        {
            List<ListDestination> destinations = new List<ListDestination>();
            string sql = "Select Id, Name, Address, Images, Rating, Cost, OpenTime, CloseTime, Tags from Destinations" + filter;
            _dao.OpenConnection();
            MySqlDataReader reader = _dao.ExecuteQuery(sql, null);
            while (reader.Read())
            {
                ListDestination destination = new ListDestination(reader);
                destinations.Add(destination);
            }
            _dao.CloseConnection();
            return destinations;
        }
        /// <summary>
        /// Get detail of destination by destination id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DestinationDetail? GetDestinationById(int id)
        {
            string sql = "Select DestinationId, Name, LocalName, Address, Images, Cost, OpenTime" +
                ", CloseTime, Tags, Introduction, GoogleMapUrl, Rating from Destinations where DestinationId = @id";
            MySqlParameter parameter = new MySqlParameter("@id", id);
            _dao.OpenConnection();
            MySqlDataReader reader = _dao.ExecuteQuery(sql, parameter);
            if (reader.Read())
            {
                DestinationDetail destination = new DestinationDetail(reader);
                _dao.CloseConnection();
                return destination;
            }
            _dao.CloseConnection();
            return null;
        }

        /// <summary>
        /// Get random destinations
        /// </summary>
        /// <param name="limit"></param>
        /// <returns> Default return 3 destinations</returns>
        public List<HomeDestination> GetRandomDestinations(int limit = 3)
        {
            List<HomeDestination> destinations = new List<HomeDestination>();
            string sql = "Select DestinationId, Name, Address, Images, Rating from Destinations order by RAND() limit @limit";
            MySqlParameter parameter = new MySqlParameter("@limit", limit);
            _dao.OpenConnection();
            MySqlDataReader reader = _dao.ExecuteQuery(sql, parameter);
            while (reader.Read())
            {
                HomeDestination destination = new HomeDestination(reader);
                destinations.Add(destination);
            }
            _dao.CloseConnection();
            return destinations;
        }

        /// <summary>
        /// Get destinations for admin page
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<DestinationElement> GetDestinationElements(string sql)
        {
            List<DestinationElement> destinations = new List<DestinationElement>();
            _dao.OpenConnection();
            MySqlDataReader reader = _dao.ExecuteQuery(sql);
            while (reader.Read())
            {
                DestinationElement destination = new DestinationElement(reader);
                destinations.Add(destination);
            }
            _dao.CloseConnection();
            return destinations;
        }

        /// <summary>
        /// Add destination
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        public int AddDestination(DestinationModel destination)
        {
            string sql = "Insert into Destinations(Name, LocalName, Address, GoogleMapUrl, Cost, Opentime, CloseTime, Images, Tags, Introduction)" +
                "values (@name, @localName, @address, @googleMapUrl, @cost, @openTime, @closeTime, @images, @tags, @introduction)";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@name", destination.Name),
                new MySqlParameter("@localName", destination.LocalName),
                new MySqlParameter("@address", destination.Address),
                new MySqlParameter("@googleMapUrl", destination.GoogleMapUrl),
                new MySqlParameter("@cost", destination.Cost),
                new MySqlParameter("@openTime", destination.OpenTime),
                new MySqlParameter("@closeTime", destination.CloseTime),
                new MySqlParameter("@images", String.Join(";",destination.Images)),
                new MySqlParameter("@tags", destination.Tags),
                new MySqlParameter("@introduction", destination.Introduction)
            };
            _dao.OpenConnection();
            _dao.ExecuteNonQuery(sql, parameters);
            _dao.ExecuteNonQuery(sql, parameters);
            sql = "Select SCOPE_IDENTITY();";
            int id = ((int?)_dao.ExecuteScalar(sql)).Value;
            _dao.CloseConnection();
            return id;
        }

        /// <summary>
        /// Update destination
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        public int UpdateDestination(DestinationModel destination)
        {
            string sql = "Update Destinations set Name = @name, LocalName = @localName, Address = @address, " +
                "GoogleMapUrl = @googleMapUrl,  = @close_time, open_day = @open_day, destination_html = @destination_html, " +
                "destination_image_url = @destination_image_url, rating = @rating where destination_id = @destination_id";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@name", destination.Name),
                new MySqlParameter("@localName", destination.LocalName),
                new MySqlParameter("@address", destination.Address),
                new MySqlParameter("@googleMapUrl", destination.GoogleMapUrl),
                new MySqlParameter("@cost", destination.Cost),
                new MySqlParameter("@openTime", destination.OpenTime),
                new MySqlParameter("@closeTime", destination.CloseTime),
                new MySqlParameter("@images", String.Join(";",destination.Images)),
                new MySqlParameter("@tags", destination.Tags),
                new MySqlParameter("@introduction", destination.Introduction)
            };

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
