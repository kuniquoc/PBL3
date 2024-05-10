using MySqlConnector;
using DaNangTourism.Server.Models;

namespace DaNangTourism.Server.DAL
{
    public class ScheduleDAO
    {
        public Dictionary<int, Schedule> GetAllSchedule()
        {
            DAO dao = DAO.Instance;
            string sql = "Select * from schedules";
            MySqlDataReader reader = dao.ExecuteQuery(sql, null);
            Dictionary<int, Schedule> schedules = new Dictionary<int, Schedule>();
            while (reader.Read())
            {
                Schedule schedule = new Schedule();
                schedule.Id = reader.GetInt32("schedule_id");
                schedule.UserId = reader.GetInt32("user_id");
                schedule.Name = reader.GetString("name");
                schedule.Describe = reader.GetString("describe");
                string s = reader.GetString(reader.GetOrdinal("status"));
                schedule.Status = (ScheduleStatus)Enum.Parse(typeof(ScheduleStatus), s); ;
                schedule.IsPublic = reader.GetBoolean("is_public");
                schedules.Add(schedule.Id, schedule);
            }
            return schedules;
        }
        public Schedule GetScheduleById(int id)
        {
            DAO dao = DAO.Instance;
            string sql = "Select * from schedules where schedule_id = @id";
            MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@id", id) };
            MySqlDataReader reader = dao.ExecuteQuery(sql, parameters);
            Schedule schedule = new Schedule();
            if (reader.Read())
            {
                schedule.Id = reader.GetInt32("schedule_id");
                schedule.UserId = reader.GetInt32("user_id");
                schedule.Name = reader.GetString("name");
                schedule.Describe = reader.GetString("describe");
                string s = reader.GetString(reader.GetOrdinal("status"));
                schedule.Status = (ScheduleStatus)Enum.Parse(typeof(ScheduleStatus), s); ;
                schedule.IsPublic = reader.GetBoolean("is_public");
            }
            return schedule;
        }

        public int UpdateSchedule(Schedule schedule)
        {
            DAO dao = DAO.Instance;
            string sql = "Update schedules set schedule_name = @scheduleName, schedule_describe = @scheduleDescribe, " +
                "status = @status, is_public = @isPublic where schedule_id = @scheduleId";
            MySqlParameter[] parameters = new MySqlParameter[5];
            parameters[0] = new MySqlParameter("@scheduleName", schedule.Name);
            parameters[1] = new MySqlParameter("@scheduleDescribe", schedule.Describe);
            parameters[2] = new MySqlParameter("@status", schedule.Status);
            parameters[3] = new MySqlParameter("@isPublic", schedule.IsPublic);
            parameters[4] = new MySqlParameter("@scheduleId", schedule.Id);
            return dao.ExecuteNonQuery(sql, parameters);
        }
        public int DeleteDestination(int id)
        {
            DAO dao = DAO.Instance;
            string sql = "Delete from schedules where schedule_id = @id";
            MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@id", id) };
            return dao.ExecuteNonQuery(sql, parameters);
        }
    }
}
