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
        int CloneSchedule(int userId, string creator);
        bool IsCreator(int userId, int scheduleId);
        int AddScheduleDestination(int userId, ScheduleDestination destination);
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
            string sql = "INSERT INTO Schedules (UserId, Title, Description, UpdateAt, Creator, IsPublic) " +
                "VALUES (@userId, @title, @description, @updateAt, @creator, @isPublic); SELECT LAST_INSERT_ID();";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@userId", userId),
                new MySqlParameter("@title", schedule.Title),
                new MySqlParameter("@description", schedule.Description),
                new MySqlParameter("@updateAt", DateTime.Now),
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


        /// <summary>
        /// Clone schedule
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        public int CloneSchedule(int userId, string creator) {
            string sql = "INSERT INTO Schedules (UserId, Status, Title, Title, Description, StartDate, TotalDays, TotalDays, TotalBudget, UpdateAt, Creator, IsPucblic) " +
                "SELECT @userId, Status, Title, Description, StartDate, TotalDays, TotalBudget, UpdateAt, @creator, IsPucblic; SELECT LAST_INSERT_ID();";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@userId", userId),
                new MySqlParameter("@creator", creator)
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

    }
}
