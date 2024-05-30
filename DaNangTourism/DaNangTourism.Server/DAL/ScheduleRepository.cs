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
        int CreateSchedule(int userId, string creator, InputSchedule schedule);
        void UpdateBudget(int scheduleId, double budget);
        int CloneSchedule(int userId, string creator, int scheduleId);
        bool IsCreator(int userId, int scheduleId);
        UpdateScheduleModel UpdateSchedule(int userId, int scheduleId, UpdateScheduleModel schedule);
    }
    public class ScheduleRepository: IScheduleRepository
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
            string sql = "SELECT ScheduleId, Status, Title, Description, StartDate, TotalDays, TotalBudget, UpdatedAt, Creator, IsPublic " +
                "FROM Schedules WHERE ScheduleId = @scheduleId AND UserId = @userId";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@scheduleId", scheduleId),
                new MySqlParameter("@userId", userId)
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
        public int CreateSchedule(int userId, string creator, InputSchedule schedule)
        {
            string sql = "INSERT INTO Schedules (UserId, Title, Description, StartDate, UpdatedAt, Creator, IsPublic) " +
                "VALUES (@userId, @title, @description, @startDate, @updatedAt, @creator, @isPublic); SELECT LAST_INSERT_ID();";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@userId", userId),
                new MySqlParameter("@title", schedule.Title),
                new MySqlParameter("@description", schedule.Description),
                new MySqlParameter("@startDate", DateOnly.FromDateTime(DateTime.Now)),
                new MySqlParameter("@updatedAt", DateTime.Now),
                new MySqlParameter("@creator", creator),
                new MySqlParameter("@isPublic", schedule.IsPublic)
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

        public void UpdateBudget(int scheduleId, double budget)
        {
            string sql = "UPDATE Schedules SET TotalBudget = TotalBudget + @budget WHERE ScheduleID = @scheduleId ";
            MySqlParameter[] parameters = new MySqlParameter[]{
                new MySqlParameter("@budget", budget),
                new MySqlParameter("@scheduleId", scheduleId)
            };
            using (var con = new MySqlConnection(_connectionString))
            {
                con.Open();
                using (var command = new MySqlCommand(sql, con))
                {
                    command.Parameters.AddRange(parameters);
                    command.ExecuteNonQuery();
                }
            }
        }
        /// <summary>
        /// Clone schedule
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        public int CloneSchedule(int userId, string creator, int scheduleId) {
            string sql = "INSERT INTO Schedules (UserId, Status, Title, Description, StartDate, TotalDays, TotalBudget, UpdatedAt, Creator, IsPublic) " +
                "(SELECT @userId, Status, Title, Description, StartDate, TotalDays, TotalBudget, UpdatedAt, @creator, IsPublic FROM Schedules WHERE ScheduleId = @scheduleId); " +
                "SELECT LAST_INSERT_ID();";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@userId", userId),
                new MySqlParameter("@creator", creator),
                new MySqlParameter("@scheduleId", scheduleId)
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
            string sql = "SELECT COUNT(*) FROM Schedules WHERE UserId = @userId AND ScheduleId = @scheduleId";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@userId", userId),
                new MySqlParameter("@scheduleId", scheduleId)
            };
            return GetScheduleCount(sql, parameters) > 0;
        }

        /// <summary>
        /// Update schedule
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="scheduleId"></param>
        /// <param name="schedule"></param>
        public UpdateScheduleModel UpdateSchedule(int userId, int scheduleId, UpdateScheduleModel schedule)
        {
            string sql = "UPDATE Schedules SET Title = @title, Description = @description, IsPublic = @isPublic, Status = @status WHERE UserId = @userId AND ScheduleId = @scheduleId; " +
                "SELECT Title, Description, IsPublic, Status FROM Schedules WHERE ScheduleId = @scheduleId;";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@title", schedule.Title),
                new MySqlParameter("@description", schedule.Description),
                new MySqlParameter("@isPublic", schedule.IsPublic),
                new MySqlParameter("@status", schedule.Status),
                new MySqlParameter("@userId", userId),
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
                        if (reader.Read())
                        {
                            return new UpdateScheduleModel(reader);
                        }
                        throw new Exception("This schedule isn't exist");
                    }
                }
            }
        }
    }
}
