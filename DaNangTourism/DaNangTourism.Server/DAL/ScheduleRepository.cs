using MySqlConnector;
using DaNangTourism.Server.Models.ScheduleModels;

namespace DaNangTourism.Server.DAL
{
    public interface IScheduleRepository
    {
        IEnumerable<ScheduleElement> GeListSchedule(string sql, params MySqlParameter[] parameters);
        int GetScheduleCount(string sql, params MySqlParameter[] parameters);
        IEnumerable<PublicScheduleElement> GetPublicSchedule(string sql, params MySqlParameter[] parameters);
        ScheduleDetail? GetScheduleDetail(int userId, int scheduleId);
        int CreateSchedule(int userId, InputSchedule schedule);
        int CloneSchedule(int userId, int scheduleId);
        bool IsCreator(int userId, int scheduleId);
        void UpdateSchedule(int userId, int scheduleId, UpdateScheduleModel schedule);
        void DeleteSchedule(int scheduleId);
    }
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly string _connectionString;

        public ScheduleRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Get list schedule
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<ScheduleElement> GeListSchedule(string sql, params MySqlParameter[] parameters)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandText = sql;
                    command.Parameters.AddRange(parameters);
                    using (var reader = command.ExecuteReader())
                    {
                        var schedules = new List<ScheduleElement>();
                        while (reader.Read())
                        {
                            schedules.Add(new ScheduleElement(reader));
                        }
                        return schedules;
                    }
                }
            }
        }

        /// <summary>
        /// Get schedule count 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int GetScheduleCount(string sql, params MySqlParameter[] parameters)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandText = sql;
                    command.Parameters.AddRange(parameters);
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// Get public schedule
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<PublicScheduleElement> GetPublicSchedule(string sql, params MySqlParameter[] parameters)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandText = sql;
                    command.Parameters.AddRange(parameters);
                    using (var reader = command.ExecuteReader())
                    {
                        var schedules = new List<PublicScheduleElement>();
                        while (reader.Read())
                        {
                            schedules.Add(new PublicScheduleElement(reader));
                        }
                        return schedules;
                    }
                }
            }
        }

        /// <summary>
        /// Get schedule detail
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="scheduleId"></param>
        /// <returns></returns>
        public ScheduleDetail? GetScheduleDetail(int userId, int scheduleId)
        {
            string sql = "SELECT Schedules.schedule_id AS ScheduleId, status AS Status, title AS Title, description AS Description," +
                      " MIN(Date) AS StartDate, IFNULL((DATEDIFF(MAX(Date), MIN(Date)) + 1), 0) AS TotalDays, IFNULL(SUM(Budget), 0) AS TotalBudget," +
                      " updated_at AS UpdatedAt, full_name AS Creator, is_public AS IsPublic" +
                      " FROM Schedules LEFT JOIN ScheduleDestinations ON Schedules.schedule_id = ScheduleDestinations.schedule_id" +
                      " LEFT JOIN Users ON Users.user_id = Schedules.user_id" +
                      " WHERE Schedules.schedule_id = @scheduleId AND Schedules.user_id = @userId" +
                      " GROUP BY Schedules.schedule_id, status, title, description, updated_at, is_public";
            MySqlParameter[] parameters =
            {
                new ("@scheduleId", scheduleId),
                new ("@userId", userId)
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
                            return new ScheduleDetail(reader);
                        }
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// Create schedule
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="schedule"></param>
        /// <returns></returns>
        public int CreateSchedule(int userId, InputSchedule schedule)
        {
            string sql = "INSERT INTO Schedules (user_id, title, description, is_public) " +
                "VALUES (@userId, @title, @description, @isPublic); SELECT LAST_INSERT_ID();";
            MySqlParameter[] parameters =
            {
                new ("@userId", userId),
                new ("@title", schedule.Title),
                new ("@description", schedule.Description),
                new ("@isPublic", schedule.IsPublic)
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
        /// Clone schedule
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        public int CloneSchedule(int userId, int scheduleId)
        {
            string sql = "INSERT INTO Schedules (user_id, title, description) " +
                "(SELECT @userId, title, description FROM Schedules WHERE schedule_id = @scheduleId); " +
                "SELECT LAST_INSERT_ID();";
            MySqlParameter[] parameters =
            {
                new ("@userId", userId),
                new ("@scheduleId", scheduleId),
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
        /// Check user is creator
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="scheduleId"></param>
        /// <returns></returns>
        public bool IsCreator(int userId, int scheduleId)
        {
            string sql = "SELECT COUNT(*) FROM Schedules WHERE user_id = @userId AND schedule_id = @scheduleId";
            MySqlParameter[] parameters =
            {
                new ("@userId", userId),
                new ("@scheduleId", scheduleId)
            };
            return GetScheduleCount(sql, parameters) > 0;
        }

        /// <summary>
        /// Update schedule
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="scheduleId"></param>
        /// <param name="schedule"></param>
        public void UpdateSchedule(int userId, int scheduleId, UpdateScheduleModel schedule)
        {
            string sql = "UPDATE Schedules SET title = @title, description = @description, is_public = @isPublic, status = @status," +
                " updated_at = @updatedAt WHERE user_id = @userId AND schedule_id = @scheduleId;";

            MySqlParameter[] parameters =
            {
                new ("@title", schedule.Title),
                new ("@description", schedule.Description),
                new ("@isPublic", schedule.IsPublic),
                new ("@status", schedule.Status),
                new ("@userId", userId),
                new ("@scheduleId", scheduleId),
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

        public void DeleteSchedule(int scheduleId)
        {
            string sql = "DELETE FROM Schedules WHERE schedule_id = @scheduleId";
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
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
