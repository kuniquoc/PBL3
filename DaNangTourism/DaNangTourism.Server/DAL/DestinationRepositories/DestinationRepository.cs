using MySqlConnector;
using System.Net.WebSockets;
using DaNangTourism.Server.Models;

namespace DaNangTourism.Server.DAL
{
    public interface IDestinationRepository
    {
        IEnumerable<HomeDestination> GetNewestDestinations(int limit = 5);
        IEnumerable<ListDestination> GetListDestination(string filter, List<MySqlParameter> parameters);
        DestinationDetail? GetDestinationById(int id);
        IEnumerable<HomeDestination> GetRandomDestinations(int limit);
        IEnumerable<DestinationElement> GetDestinationElements(string sql);
        int GetDestinationCount(string sql);
        int AddDestination(InputDestinationModel destination);
        int UpdateDestination(InputDestinationModel destination);
        int DeleteDestination(int id);
    }
    public class DestinationRepository : IDestinationRepository
    {
        private readonly string _connectionString;
        
        public DestinationRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        /// <summary>
        /// Get limit Destination to Home Page
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        public IEnumerable<HomeDestination> GetNewestDestinations(int limit = 5)
        {
            string sql = "Select DestinationId, Name, Address, Images, Rating from Destinations order by Created_At desc limit @limit";
            MySqlParameter parameter = new MySqlParameter("@limit", limit);
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add(parameter);
                    using (var reader = command.ExecuteReader())
                    {
                        var destinations = new List<HomeDestination>();
                        while (reader.Read())
                        {
                            var destination = new HomeDestination(reader);
                            destinations.Add(destination);
                        }
                        return destinations;
                    }
                }    
            }    
        }

        /// <summary>
        /// Get list destination for destination page
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<ListDestination> GetListDestination(string filter, List<MySqlParameter> parameters)
        {
            string sql = "Select DestinationId, Name, Address, Images, Rating, Cost, OpenTime, CloseTime, Tags from Destinations" + filter;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddRange(parameters.ToArray());
                    using (var reader = command.ExecuteReader())
                    {
                        var destinations = new List<ListDestination>();
                        while (reader.Read())
                        {
                            var destination = new ListDestination(reader);
                            destinations.Add(destination);
                        }
                        return destinations;
                    }
                }
            }
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
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add(parameter);
                    using (var reader = command.ExecuteReader())
                    {
                        var destinations = new List<ListDestination>();
                        if (reader.Read())
                        {
                            var destination = new DestinationDetail(reader);
                            return destination;
                        }
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// Get random destinations
        /// </summary>
        /// <param name="limit"></param>
        /// <returns> Default return 3 destinations</returns>
        public IEnumerable<HomeDestination> GetRandomDestinations(int limit)
        {
            
            string sql = "Select DestinationId, Name, Address, Images, Rating from Destinations order by RAND() limit @limit";
            MySqlParameter parameter = new MySqlParameter("@limit", limit);
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add(parameter);
                    using (var reader = command.ExecuteReader())
                    {
                        var destinations = new List<HomeDestination>();
                        while (reader.Read())
                        {
                            HomeDestination destination = new HomeDestination(reader);
                            destinations.Add(destination);
                        }
                        return destinations;
                    }
                }
            }
        }

        /// <summary>
        /// Get destinations for admin page
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<DestinationElement> GetDestinationElements(string sql)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        var destinations = new List<DestinationElement>();

                        while (reader.Read())
                        {
                            DestinationElement destination = new DestinationElement(reader);
                            destinations.Add(destination);
                        }
                        return destinations;
                    }
                }
            }
        }
        /// <summary>
        /// Get count of Destination in GetDestinationElements method
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int GetDestinationCount(string sql)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    object? result = command.ExecuteScalar();
                    int count = result != null ? Convert.ToInt32(result) : 0;
                    return count;
                }
            }    
        }
        
        /// <summary>
        /// Add destination
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        public int AddDestination(InputDestinationModel destination)
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
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddRange(parameters);
                    int result = command.ExecuteNonQuery();
                    return result;
                }
            }
        }

        /// <summary>
        /// Update destination
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        public int UpdateDestination(InputDestinationModel destination)
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
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddRange(parameters);
                    int result = command.ExecuteNonQuery();
                    return result;
                }
            }
        }
        public int DeleteDestination(int id)
        {
            string sql = "Delete from destinations where destination_id = @id";
            MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@id", id) };
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddRange(parameters);
                    int result = command.ExecuteNonQuery();
                    return result;
                }
            }
        }
    }
}
