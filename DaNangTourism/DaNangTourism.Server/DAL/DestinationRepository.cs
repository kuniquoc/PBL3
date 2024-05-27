using MySqlConnector;
using System.Net.WebSockets;
using DaNangTourism.Server.Models.DestinationModels;
using DaNangTourism.Server.Models.ReviewModels;

namespace DaNangTourism.Server.DAL
{
    public interface IDestinationRepository
    {
        IEnumerable<HomeDestination> GetNewestDestinations(int limit = 5);
        IEnumerable<ListDestination> GetListDestination(string filter, List<MySqlParameter> parameters);
        DestinationDetail? GetDestinationById(int id);
        IEnumerable<HomeDestination> GetRandomDestinations(int limit);
        IEnumerable<DestinationElement> GetDestinationElements(string sql, params MySqlParameter[] parameters);
        int GetDestinationCount(string sql, params MySqlParameter[] parameters);
        int AddDestination(InputDestinationModel destination);
        int UpdateDestination(int id, InputDestinationModel destination);
        int DeleteDestination(int id);

        void UpdateRating(int DestinationId, float newRating);
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
            string sql = "SELECT DestinationId, Name, Address, Images, Rating FROM Destinations ORDER BY Created_At DESC LIMIT @limit";
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
                            destinations.Add(new HomeDestination(reader));
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
        public IEnumerable<ListDestination> GetListDestination(string sql, List<MySqlParameter> parameters)
        {
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
                            destinations.Add(new ListDestination(reader));
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
            string sql = "SELECT DestinationId, Name, LocalName, Address, Images, Cost, OpenTime" +
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
                            return new DestinationDetail(reader);
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
            
            string sql = "SELECT DestinationId, Name, Address, Images, Rating FROM Destinations ORDER BY RAND() LIMIT @limit";
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
                            destinations.Add(new HomeDestination(reader));
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
        public IEnumerable<DestinationElement> GetDestinationElements(string sql, params MySqlParameter[] parameters)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddRange(parameters);
                    using (var reader = command.ExecuteReader())
                    {
                        var destinations = new List<DestinationElement>();

                        while (reader.Read())
                        {
                            destinations.Add(new DestinationElement(reader));
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
        public int GetDestinationCount(string sql, params MySqlParameter[] parameters)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddRange(parameters);
                    int? count =(int?) command.ExecuteScalar();
                    return Convert.ToInt32(count);
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
            string sql = "INSERT INTO Destinations(Name, LocalName, Address, GoogleMapUrl, Cost, Opentime, CloseTime, Images, Tags, Introduction)" +
                "VALUES (@name, @localName, @address, @googleMapUrl, @cost, @openTime, @closeTime, @images, @tags, @introduction)";
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
                    return command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Update destination
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        public int UpdateDestination(int id, InputDestinationModel destination)
        {
            string sql = "UPDATE Destinations SET Name = @name, LocalName = @localName, Address = @address, GoogleMapUrl = @googleMapUrl, Cost = @cost, Opentime = @openTime, " +
                   "CloseTime = @closeTime, Images = @images, Tags = @tags, Introduction = @introduction WHERE DestinationId = @destinationId";
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
                new MySqlParameter("@introduction", destination.Introduction),
                new MySqlParameter("@destinationId", id)
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
        /// Delete destination
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteDestination(int id)
        {
            string sql = "DELETE FROM Destinations WHERE DestinationId = @id";
            MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@id", id) };
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
        /// Get rating of destination
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double GetRating(int id)
        {
            string sql = "SELECT Rating FROM Destinations WHERE DestinationId = @id";
            MySqlParameter parameter = new MySqlParameter("@id", id);
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add(parameter);
                    return Convert.ToDouble(command.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// increase or decrease rating of destination
        /// </summary>
        /// <param name="review"></param>
        /// <param name="isAdding"></param>
        public void UpdateRating(int destinationId, float newRating)
        {
            string sql = "UPDATE Destinations SET Rating = @rating WHERE DestinationId = @destinationId";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                    new MySqlParameter("@rating", newRating),
                    new MySqlParameter("@destinationId", destinationId)
            };
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddRange(parameters);
                    command.ExecuteNonQuery();
                }

            }

        }
    }
}
