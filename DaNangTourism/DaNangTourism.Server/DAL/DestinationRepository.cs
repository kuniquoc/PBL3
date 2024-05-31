using MySqlConnector;
using System.Net.WebSockets;
using DaNangTourism.Server.Models.DestinationModels;
using DaNangTourism.Server.Models.ReviewModels;
using System.Text;

namespace DaNangTourism.Server.DAL
{
  public interface IDestinationRepository
  {
    IEnumerable<HomeDestination> GetNewestDestinations(int limit = 10);
    IEnumerable<ListDestinationItem> GetListDestination(string filter, List<MySqlParameter> parameters);
    DestinationDetail? GetDestinationById(int id, int userId = 0);
    IEnumerable<HomeDestination> GetRandomDestinations(int limit);
    IEnumerable<DestinationElement> GetDestinationElements(string sql, params MySqlParameter[] parameters);
    int GetDestinationCount(string sql, params MySqlParameter[] parameters);
    int AddDestination(InputDestinationModel destination);
    InputDestinationModel GetDestinationToUpdate(int id);
    InputDestinationModel UpdateDestination(int id, InputDestinationModel destination);
    int DeleteDestination(int id);
    string GetName(int DestinationId);
    string GetAddress(int DestinationId);
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
    public IEnumerable<HomeDestination> GetNewestDestinations(int limit = 10)
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
    public IEnumerable<ListDestinationItem> GetListDestination(string sql, List<MySqlParameter> parameters)
    {
      using (var connection = new MySqlConnection(_connectionString))
      {
        connection.Open();
        using (var command = new MySqlCommand(sql, connection))
        {
          command.Parameters.AddRange(parameters.ToArray());
          using (var reader = command.ExecuteReader())
          {
            var destinations = new List<ListDestinationItem>();
            while (reader.Read())
            {
              destinations.Add(new ListDestinationItem(reader));
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
    public DestinationDetail? GetDestinationById(int id, int userId = 0)
    {
      StringBuilder sql = new StringBuilder();
      List<MySqlParameter> parameters = new List<MySqlParameter>();
      if (userId > 0)
      {
        sql.Append("SELECT Destinations.DestinationId, Name, LocalName, Address, Images, Cost, OpenTime, CloseTime, Tags, Introduction, GoogleMapUrl, Rating");
      }
      else
      {
        sql.Append("SELECT DestinationId, Name, LocalName, Address, Images, Cost, OpenTime, CloseTime, Tags, Introduction, GoogleMapUrl, Rating");
      }

      if (userId > 0)
      {
        sql.Append(", IF(UserId = @userId, TRUE, FALSE) as Favorite FROM Destinations LEFT JOIN FavoriteDestinations ON FavoriteDestinations.DestinationId = Destinations.DestinationId ");
        parameters.Add(new MySqlParameter("@userId", userId));
      }
      else
      {
        sql.Append(", FALSE AS Favorite FROM Destinations");
      }

      if (userId > 0)
      {
        sql.Append(" WHERE Destinations.DestinationId = @id");
      }
      else
      {
        sql.Append(" WHERE DestinationId = @id");
      }

      parameters.Add(new MySqlParameter("@id", id));
      using (var connection = new MySqlConnection(_connectionString))
      {
        connection.Open();
        using (var command = new MySqlCommand(sql.ToString(), connection))
        {
          command.Parameters.AddRange(parameters.ToArray());
          using (var reader = command.ExecuteReader())
          {
            var destinations = new List<ListDestinationItem>();
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
          return Convert.ToInt32(command.ExecuteScalar());
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
      string sql = "INSERT INTO Destinations(Name, LocalName, Address, GoogleMapUrl, Cost, OpenTime, CloseTime, Images, Tags, Introduction)" +
          "VALUES (@name, @localName, @address, @googleMapUrl, @cost, @openTime, @closeTime, @images, @tags, @introduction); SELECT LAST_INSERT_ID();";
      MySqlParameter[] parameters =
      [
          new("@name", destination.Name),
                new("@localName", destination.LocalName),
                new("@address", destination.Address),
                new("@googleMapUrl", destination.GoogleMapUrl),
                new("@cost", destination.Cost),
                new("@openTime", destination.OpenTime),
                new("@closeTime", destination.CloseTime),
                new("@images", string.Join(";",destination.Images)),
                new("@tags", string.Join(";",destination.Tags)),
                new("@introduction", destination.Introduction),
            ];
      using (var connection = new MySqlConnection(_connectionString))
      {
        connection.Open();
        using (var command = new MySqlCommand(sql, connection))
        {
          command.Parameters.AddRange(parameters);
          return Convert.ToInt32(command.ExecuteScalar());
        }
      }
    }

    public InputDestinationModel GetDestinationToUpdate(int id)
    {
      string sql = "SELECT Name, LocalName, Address, GoogleMapUrl, Cost, OpenTime, CloseTime, Images, Tags, Introduction FROM Destinations WHERE DestinationId = @id";
      MySqlParameter parameter = new MySqlParameter("@id", id);
      using (var connection = new MySqlConnection(_connectionString))
      {
        connection.Open();
        using (var command = new MySqlCommand(sql, connection))
        {
          command.Parameters.Add(parameter);
          using (var reader = command.ExecuteReader())
          {
            if (reader.Read())
            {
              return new InputDestinationModel(reader);
            }
            throw new Exception("Destination doesn't exist");
          }
        }
      }
    }

    /// <summary>
    /// Update destination
    /// </summary>
    /// <param name="destination"></param>
    /// <returns></returns>
    public InputDestinationModel UpdateDestination(int id, InputDestinationModel destination)
    {
      string sql = "UPDATE Destinations SET Name = @name, LocalName = @localName, Address = @address, GoogleMapUrl = @googleMapUrl, Cost = @cost, Opentime = @openTime, " +
             "CloseTime = @closeTime, Images = @images, Tags = @tags, Introduction = @introduction WHERE DestinationId = @destinationId; " +
             "SELECT Name, LocalName, Address, GoogleMapUrl, Cost, OpenTime, CloseTime, Images, Tags, Introduction FROM Destinations " +
             "WHERE DestinationId = @destinationId";
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
                new MySqlParameter("@tags", String.Join(";",destination.Tags)),
                new MySqlParameter("@introduction", destination.Introduction),
                new MySqlParameter("@destinationId", id)
      };
      using (var connection = new MySqlConnection(_connectionString))
      {
        connection.Open();
        using (var command = new MySqlCommand(sql, connection))
        {

          command.Parameters.AddRange(parameters);
          using (var reader = command.ExecuteReader())
          {
            if (reader.Read())
            {
              return new InputDestinationModel(reader);
            }
            else throw new Exception("Fault occurs when get destination which updated");
          }
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
    /// Get Destination Name by Destination Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public string GetName(int destinationId)
    {
      string sql = "SELECT Name FROM Destinations WHERE DestinationId = @id";
      MySqlParameter parameter = new MySqlParameter("@id", destinationId);
      using (var connection = new MySqlConnection(_connectionString))
      {
        connection.Open();
        using (var command = new MySqlCommand(sql, connection))
        {
          command.Parameters.Add(parameter);
          using (var reader = command.ExecuteReader())
          {
            if (reader.Read())
            {
              return reader.GetString(0);
            }
            throw new Exception("Destination doesn't exist");
          }
        }
      }
    }

    /// <summary>
    /// Get address by destinationId
    /// </summary>
    /// <param name="destinationId"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public string GetAddress(int destinationId)
    {
      string sql = "SELECT Address FROM Destinations WHERE DestinationId = @id";
      MySqlParameter parameter = new MySqlParameter("@id", destinationId);
      using (var connection = new MySqlConnection(_connectionString))
      {
        connection.Open();
        using (var command = new MySqlCommand(sql, connection))
        {
          command.Parameters.Add(parameter);
          using (var reader = command.ExecuteReader())
          {
            if (reader.Read())
            {
              return reader.GetString(0);
            }
            throw new Exception("Destination doesn't exist");
          }
        }
      }
    }

  }
}
