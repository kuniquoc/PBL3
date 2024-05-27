using MySqlConnector;
using DaNangTourism.Server.Models.ScheduleModels;

namespace DaNangTourism.Server.DAL
{
    public interface IScheduleRepository
    {
        IEnumerable<ScheduleElement> GeListSchedule(string sql, params MySqlParameter[] parameters);
        int GetScheduleCount(string sql, params MySqlParameter[] parameters);
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
        //public List<ScheduleDetail> GetSchedulesByUserId(int userId)
        //{
        //    string sql = "Select * from schedules where user_id = @userId";
        //    MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@userId", userId) };
        //    MySqlDataReader reader = _dao.ExecuteQuery(sql, parameters);
        //    List<ScheduleDetail> schedules = new List<ScheduleDetail>();
        //    _dao.OpenConnection();
        //    if (reader.Read())
        //    {
        //        ScheduleDetail schedule = new Schedule(reader);
        //        schedules.Add(schedule);
        //    }
        //    _dao.CloseConnection();
        //    return schedules;
        //}

        //public ScheduleDetail? GetScheduleById(int id)
        //{
        //    string sql = "Select * from schedules where schedule_id = @id";
        //    MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@id", id) };
        //    MySqlDataReader reader = _dao.ExecuteQuery(sql, parameters);

        //    _dao.OpenConnection();
        //    if (reader.Read())
        //    {
        //        ScheduleDetail schedule = new Schedule(reader);
        //        _dao.CloseConnection();
        //        return schedule;
        //    }
        //    _dao.CloseConnection();
        //    return null;
        //}

        //public int AddSchedule(int userId, ScheduleDetail schedule)
        //{
        //    string sql = "Insert into schedules(user_id, schedule_name, schedule_describe, status, is_public)" +
        //        "values (@userId, @scheduleName, @scheduleDescribe, @status, @isPublic)";
        //    MySqlParameter[] parameters = new MySqlParameter[5];
        //    parameters[0] = new MySqlParameter("@userId", userId);
        //    parameters[1] = new MySqlParameter("@scheduleName", schedule.Name);
        //    parameters[2] = new MySqlParameter("@scheduleDescribe", schedule.Describe);
        //    parameters[3] = new MySqlParameter("@status", schedule.Status);
        //    parameters[4] = new MySqlParameter("@isPublic", schedule.IsPublic);
        //    _dao.OpenConnection();
        //    int result = _dao.ExecuteNonQuery(sql, parameters);
        //    _dao.CloseConnection();
        //    return result;
        //}
        //public int UpdateSchedule(ScheduleDetail schedule)
        //{
        //    string sql = "Update schedules set schedule_name = @scheduleName, schedule_describe = @scheduleDescribe, " +
        //        "status = @status, is_public = @isPublic where schedule_id = @scheduleId";
        //    MySqlParameter[] parameters = new MySqlParameter[5];
        //    parameters[0] = new MySqlParameter("@scheduleName", schedule.Name);
        //    parameters[1] = new MySqlParameter("@scheduleDescribe", schedule.Describe);
        //    parameters[2] = new MySqlParameter("@status", schedule.Status);
        //    parameters[3] = new MySqlParameter("@isPublic", schedule.IsPublic);
        //    parameters[4] = new MySqlParameter("@scheduleId", schedule.Id);
        //    _dao.OpenConnection();
        //    int result = _dao.ExecuteNonQuery(sql, parameters);
        //    _dao.CloseConnection();
        //    return result;
        //}
        //public int DeleteSchedule(int id)
        //{
        //    string sql = "Delete from schedules where schedule_id = @id";
        //    MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@id", id) };
        //    _dao.OpenConnection();
        //    int result = _dao.ExecuteNonQuery(sql, parameters);
        //    _dao.CloseConnection();
        //    return result;
        //}
    }
}
