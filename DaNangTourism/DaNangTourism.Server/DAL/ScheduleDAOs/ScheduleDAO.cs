using MySqlConnector;
using DaNangTourism.Server.Models;

namespace DaNangTourism.Server.DAL
{
    public class ScheduleDAO
    {
        private readonly DAO _dao;
        private static ScheduleDAO _instance;
        public static ScheduleDAO Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ScheduleDAO(DAO.Instance);
                }    
                return _instance;
            }
            private set { }
        }
        private ScheduleDAO(DAO dao)
        {
            _dao = dao;
        }

        public List<Schedule> GetAllSchedule()
        {
            string sql = "Select * from schedules";
            MySqlDataReader reader = _dao.ExecuteQuery(sql, null);
            List<Schedule> schedules = new List<Schedule>();
            _dao.OpenConnection();
            while (reader.Read())
            {
                Schedule schedule = new Schedule(reader);
                schedules.Add(schedule);
            }
            _dao.CloseConnection();
            return schedules;
        }
        public List<Schedule> GetSchedulesByUserId(int userId)
        {
            string sql = "Select * from schedules where user_id = @userId";
            MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@userId", userId) };
            MySqlDataReader reader = _dao.ExecuteQuery(sql, parameters);
            List<Schedule> schedules = new List<Schedule>();
            _dao.OpenConnection();
            if (reader.Read())
            {
                Schedule schedule = new Schedule(reader);
                schedules.Add(schedule);
            }
            _dao.CloseConnection();
            return schedules;
        }

        public Schedule? GetScheduleById(int id)
        {
            string sql = "Select * from schedules where schedule_id = @id";
            MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@id", id) };
            MySqlDataReader reader = _dao.ExecuteQuery(sql, parameters);

            _dao.OpenConnection();
            if (reader.Read())
            {
                Schedule schedule = new Schedule(reader);
                _dao.CloseConnection();
                return schedule;
            }
            _dao.CloseConnection();
            return null;
        }

        public int AddSchedule(int userId, Schedule schedule)
        {
            string sql = "Insert into schedules(user_id, schedule_name, schedule_describe, status, is_public)" +
                "values (@userId, @scheduleName, @scheduleDescribe, @status, @isPublic)";
            MySqlParameter[] parameters = new MySqlParameter[5];
            parameters[0] = new MySqlParameter("@userId", userId);
            parameters[1] = new MySqlParameter("@scheduleName", schedule.Name);
            parameters[2] = new MySqlParameter("@scheduleDescribe", schedule.Describe);
            parameters[3] = new MySqlParameter("@status", schedule.Status);
            parameters[4] = new MySqlParameter("@isPublic", schedule.IsPublic);
            _dao.OpenConnection();
            int result = _dao.ExecuteNonQuery(sql, parameters);
            _dao.CloseConnection();
            return result;
        }
        public int UpdateSchedule(Schedule schedule)
        {
            string sql = "Update schedules set schedule_name = @scheduleName, schedule_describe = @scheduleDescribe, " +
                "status = @status, is_public = @isPublic where schedule_id = @scheduleId";
            MySqlParameter[] parameters = new MySqlParameter[5];
            parameters[0] = new MySqlParameter("@scheduleName", schedule.Name);
            parameters[1] = new MySqlParameter("@scheduleDescribe", schedule.Describe);
            parameters[2] = new MySqlParameter("@status", schedule.Status);
            parameters[3] = new MySqlParameter("@isPublic", schedule.IsPublic);
            parameters[4] = new MySqlParameter("@scheduleId", schedule.Id);
            _dao.OpenConnection();
            int result = _dao.ExecuteNonQuery(sql, parameters);
            _dao.CloseConnection();
            return result;
        }
        public int DeleteSchedule(int id)
        {
            string sql = "Delete from schedules where schedule_id = @id";
            MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@id", id) };
            _dao.OpenConnection();
            int result = _dao.ExecuteNonQuery(sql, parameters);
            _dao.CloseConnection();
            return result;
        }
    }
}
