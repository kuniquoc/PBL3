using MySqlConnector;
using DaNangTourism.Server.Models.ScheduleModels;

namespace DaNangTourism.Server.DAL
{
    public interface IScheduleDestinationRepository
    {
        IEnumerable<ScheduleDay> GetScheduleDay(int scheduleId);
        void CloneScheduleDestination(int scheduleId, int newScheduleId);
        int AddScheduleDestination(AddScheduleDestinationModel destination, string name, string address);
        int GetScheduleId(int scheduleDestinationId);
        double GetBudget(int scheduleDestinationId);
        void DeleteScheduleDestination(int scheduleDestinationId);
        AddScheduleDestinationModel UpdateScheduleDestination(int scheduleDestinationId, UpdateScheduleDestinationModel scheduleDestination);
        string[] GetListDesNameByScheduleId(int scheduleId);
    }
    public class ScheduleDestinationRepository : IScheduleDestinationRepository
    {
        private readonly string _connectionString;
        public ScheduleDestinationRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        /// <summary>
        /// Get schedule day
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <returns></returns>
        public IEnumerable<ScheduleDay> GetScheduleDay(int scheduleId)
        {
            var sql = "SELECT ScheduleDestinationId, DestinationId, Date, Name, Address, ArrivalTime, LeaveTime, Budget, Note " +
                "FROM ScheduleDestinations WHERE ScheduleId = @scheduleId ORDER BY Date, ArrivalTime";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@scheduleId", scheduleId)
            };
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddRange(parameters);
                    using (var reader = command.ExecuteReader())
                    {
                        var scheduleDaysDict = new SortedDictionary<DateOnly, ScheduleDay>();
                        while (reader.Read())
                        {
                            var date = reader.GetDateOnly(reader.GetOrdinal("Date"));
                            if (!scheduleDaysDict.TryGetValue(date, out var scheduleDay))
                            {
                                scheduleDay = new ScheduleDay(date);
                                scheduleDaysDict[date] = scheduleDay;
                            }
                            scheduleDay.Destinations.Add(new DestinationOfDay(reader));
                        }
                        return scheduleDaysDict.Values.ToList();
                    }
                }
            }
        }

        /// <summary>
        /// Clone schedule destination
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <param name="newScheduleId"></param>
        public void CloneScheduleDestination(int scheduleId, int newScheduleId)
        {
            var sql = "INSERT INTO ScheduleDestinations (ScheduleId, DestinationId, Date, Name, Address, ArrivalTime, LeaveTime, Budget, Note) " +
                "(SELECT @newScheduleId, DestinationId, Date, Name, Address, ArrivalTime, LeaveTime, Budget, Note " +
                "FROM ScheduleDestinations WHERE ScheduleId = @scheduleId)";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@scheduleId", scheduleId),
                new MySqlParameter("@newScheduleId", newScheduleId)
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


        /// <summary>
        /// Add schedule destination
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        public int AddScheduleDestination(AddScheduleDestinationModel destination, string name, string address)
        {
            string sql = "INSERT INTO ScheduleDestinations (ScheduleId, DestinationId, Date, Name, Address, ArrivalTime, LeaveTime, Budget, Note) " +
                "VALUES (@scheduleId, @destinationId, @date, @name, @address, @arrivalTime, @leaveTime, @budget, @note); SELECT LAST_INSERT_ID();";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@scheduleId", destination.ScheduleId),
                new MySqlParameter("@destinationId", destination.DestinationId),
                new MySqlParameter("@date", destination.Date),
                new MySqlParameter("@name", name),
                new MySqlParameter("@address", address),
                new MySqlParameter("@arrivalTime", destination.ArrivalTime),
                new MySqlParameter("@leaveTime", destination.LeaveTime),
                new MySqlParameter("@budget", destination.Budget),
                new MySqlParameter("@note", destination.Note)
            };
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
        /// Get schedule id
        /// </summary>
        /// <param name="ScheduleDestinationId"></param>
        /// <returns></returns>
        public int GetScheduleId(int scheduleDestinationId)
        {
            string sql = "SELECT ScheduleId FROM ScheduleDestinations WHERE ScheduleDestinationId = @ScheduleDestinationId";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@ScheduleDestinationId", scheduleDestinationId),
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
                            return reader.GetInt32(reader.GetOrdinal("ScheduleId"));
                        }
                        throw new Exception("Don't exist this destination");
                    }
                }
            }
        }


        public double GetBudget(int scheduleDestinationId)
        {
            string sql = "SELECT Budget FROM ScheduleDestinations WHERE ScheduleDestinationId = @ScheduleDestinationId";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@ScheduleDestinationId", scheduleDestinationId),
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
                            return reader.GetDouble(reader.GetOrdinal("Budget"));
                        }
                        throw new Exception("Don't exist this destination");
                    }
                }
            }
        }

        /// <summary>
        /// Delete schedule destination
        /// </summary>
        /// <param name="scheduleDestinationId"></param>
        public void DeleteScheduleDestination(int scheduleDestinationId)
        {
            string sql = "DELETE FROM ScheduleDestinations WHERE ScheduleDestinationId = @scheduleDestinationId";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@scheduleDestinationId", scheduleDestinationId),
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

        /// <summary>
        /// Update schedule destination
        /// </summary>
        /// <param name="scheduleDestinationId"></param>
        /// <param name="scheduleDestination"></param>
        public AddScheduleDestinationModel UpdateScheduleDestination(int scheduleDestinationId, UpdateScheduleDestinationModel scheduleDestination)
        {

            string sql = "UPDATE ScheduleDestinations SET Date = @date, ArrivalTime = @arrivalTime, LeaveTime = @leaveTime, Budget = @budget, Note = @note " +
            "WHERE ScheduleDestinationId = @scheduleDestinationId; " +
            "SELECT ScheduleId, DestinationId, Date, ArrivalTime, LeaveTime, Budget, Note FROM ScheduleDestinations " +
            "WHERE ScheduleDestinationId = @scheduleDestinationId;";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
            new MySqlParameter("@date", scheduleDestination.Date),
            new MySqlParameter("@arrivalTime", scheduleDestination.ArrivalTime),
            new MySqlParameter("@leaveTime", scheduleDestination.LeaveTime),
            new MySqlParameter("@budget", scheduleDestination.Budget),
            new MySqlParameter("@note", scheduleDestination.Note),
            new MySqlParameter("@scheduleDestinationId", scheduleDestinationId)
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
                            try
                            {
                                return new AddScheduleDestinationModel(reader);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                throw new Exception("abe");
                            }
                        }
                        throw new Exception("This schedule destination isn't exist");
                    }
                }
            }

        }

        public string[] GetListDesNameByScheduleId(int scheduleId)
        {
            string sql = "SELECT Name FROM ScheduleDestinations WHERE ScheduleId = @scheduleId";
            MySqlParameter parameter = new MySqlParameter("@scheduleId", scheduleId);
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add(parameter);
                    using (var reader = command.ExecuteReader())
                    {
                        List<string> listDesName = new List<string>();
                        while (reader.Read())
                        {
                            listDesName.Add(reader.GetString(reader.GetOrdinal("Name")));
                        }
                        return listDesName.ToArray();
                    }
                }
            }
        }
    }
}
