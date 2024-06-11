using MySqlConnector;
using DaNangTourism.Server.Models.ScheduleModels;

namespace DaNangTourism.Server.DAL
{
    public interface IScheduleDestinationRepository
    {
        IEnumerable<ScheduleDay> GetScheduleDay(int scheduleId);
        void CloneScheduleDestination(int scheduleId, int newScheduleId);
        bool CheckTimeExist(int scheduleId, DateOnly date, TimeOnly arrivalTime, TimeOnly leaveTime, int scheduleDestinationId = 0);
        int AddScheduleDestination(AddScheduleDestinationModel destination);
        int GetScheduleId(int scheduleDestinationId);
        void DeleteScheduleDestination(int scheduleDestinationId);
        void UpdateScheduleDestination(int scheduleDestinationId, UpdateScheduleDestinationModel scheduleDestination);
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
            var sql = "SELECT schedule_destination_id AS ScheduleDestinationId, ScheduleDestinations.destination_id AS DestinationId, date AS Date, name AS Name, address AS Address," +
                " arrival_time AS ArrivalTime, leave_time AS LeaveTime, budget AS Budget, note AS Note " +
                "FROM ScheduleDestinations LEFT JOIN Destinations ON Destinations.destination_id = ScheduleDestinations.destination_id " +
                "WHERE schedule_id = @scheduleId ORDER BY date, arrival_time";
            MySqlParameter[] parameters =
            {
                new ("@scheduleId", scheduleId)
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
                        return scheduleDaysDict.Values;
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
            var sql = "INSERT INTO ScheduleDestinations (schedule_id, destination_id, date, arrival_time, leave_time, budget, note) " +
                "(SELECT @newScheduleId, destination_id, date, arrival_time, leave_time, budget, note " +
                "FROM ScheduleDestinations WHERE schedule_id = @scheduleId)";
            MySqlParameter[] parameters =
            {
                new ("@scheduleId", scheduleId),
                new ("@newScheduleId", newScheduleId)
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

        public bool CheckTimeExist(int scheduleId, DateOnly date, TimeOnly arrivalTime, TimeOnly leaveTime, int scheduleDestinationId = 0)
        {
            string sql = "SELECT 1 FROM scheduledestinations WHERE schedule_id = @scheduleId AND date = @date " +
                "AND ( (arrival_time >= @arrivalTime AND arrival_time <= @leaveTime) OR (leave_Time >= @arrivalTime AND leave_time <= @leaveTime ) )";
            MySqlParameter[] parameters =
            {
          new ("@scheduleId", scheduleId),
          new ("@date", date),
          new ("@arrivalTime", arrivalTime),
          new ("@leaveTime", leaveTime)
      };

            if (scheduleDestinationId != 0)
            {
                sql += " AND schedule_destination_id != @scheduleDestinationId";
                parameters = parameters.Append(new MySqlParameter("@scheduleDestinationId", scheduleDestinationId)).ToArray();
            }

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddRange(parameters);
                    using (var reader = command.ExecuteReader())
                    {
                        // If there is a record, return true
                        return !reader.Read();
                    }
                }
            }
        }
        /// <summary>
        /// Add schedule destination
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        public int AddScheduleDestination(AddScheduleDestinationModel destination)
        {
            string sql = "INSERT INTO ScheduleDestinations (schedule_id, destination_id, date, arrival_time, leave_time, budget, note) " +
                "VALUES (@scheduleId, @destinationId, @date, @arrivalTime, @leaveTime, @budget, @note); " +
                "UPDATE Schedules SET updated_at = @updatedAt;" +
                "SELECT LAST_INSERT_ID();";
            MySqlParameter[] parameters =
            {
                new ("@scheduleId", destination.ScheduleId),
                new ("@destinationId", destination.DestinationId),
                new ("@date", destination.Date),
                new ("@arrivalTime", destination.ArrivalTime),
                new ("@leaveTime", destination.LeaveTime),
                new ("@budget", destination.Budget),
                new ("@note", destination.Note),
                new ("@updatedAt", DateTime.Now)
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
            string sql = "SELECT schedule_id FROM ScheduleDestinations WHERE schedule_destination_id = @ScheduleDestinationId;";
            MySqlParameter[] parameters =
            {
                new ("@ScheduleDestinationId", scheduleDestinationId),
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
                            return reader.GetInt32(reader.GetOrdinal("schedule_id"));
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
            string sql = "DELETE FROM ScheduleDestinations WHERE schedule_destination_id = @scheduleDestinationId;" +
                "UPDATE Schedules SET updated_at = @updatedAt;";
            MySqlParameter[] parameters =
            {
                new ("@scheduleDestinationId", scheduleDestinationId),
                new ("@updatedAt", DateTime.Now)
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
        public void UpdateScheduleDestination(int scheduleDestinationId, UpdateScheduleDestinationModel scheduleDestination)
        {

            string sql = "UPDATE ScheduleDestinations SET date = @date, arrival_time = @arrivalTime, leave_time = @leaveTime, budget = @budget, note = @note " +
            "WHERE schedule_destination_id = @scheduleDestinationId; " +
            "UPDATE Schedules SET updated_at = @updatedAt;";
            MySqlParameter[] parameters =
            {
                new ("@date", scheduleDestination.Date),
                new ("@arrivalTime", scheduleDestination.ArrivalTime),
                new ("@leaveTime", scheduleDestination.LeaveTime),
                new ("@budget", scheduleDestination.Budget),
                new ("@note", scheduleDestination.Note),
                new ("@scheduleDestinationId", scheduleDestinationId),
                new ("@updatedAt", DateTime.Now)
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

        public string[] GetListDesNameByScheduleId(int scheduleId)
        {
            string sql = "SELECT name FROM ScheduleDestinations LEFT JOIN Destinations ON Destinations.destination_id = ScheduleDestinations.destination_id WHERE schedule_id = @scheduleId";
            var parameter = new MySqlParameter("@scheduleId", scheduleId);
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add(parameter);
                    using (var reader = command.ExecuteReader())
                    {
                        var listDesName = new List<string>();
                        while (reader.Read())
                        {
                            listDesName.Add(reader.GetString(reader.GetOrdinal("name")));
                        }
                        return listDesName.ToArray();
                    }
                }
            }
        }

    }
}
