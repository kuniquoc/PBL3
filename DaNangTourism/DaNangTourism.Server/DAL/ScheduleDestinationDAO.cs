using MySqlConnector;
using DaNangTourism.Server.Models;

namespace DaNangTourism.Server.DAL
{
    public class ScheduleDestinationDAO
    {
        public Dictionary<int, ScheduleDestination> GetDestinations(int scheduleId)
        {
            DAO dao = DAO.Instance;
            string sql = "Select * from schedule_destinations where schedule_id = @scheduleId";
            MySqlParameter[] parameters = new MySqlParameter[] {new MySqlParameter("scheduleId", scheduleId)};
            MySqlDataReader reader = dao.ExecuteQuery(sql, parameters);
            Dictionary<int, ScheduleDestination> destinations = new Dictionary<int, ScheduleDestination>();
            while (reader.Read())
            {
                ScheduleDestination destination = new ScheduleDestination();
                destination.Id = reader.GetInt32("sd_id");
                destination.DestinationId = reader.GetInt32("destination_id");
                destination.ArrivalTime = reader.GetDateTime("arrival_time");
                destination.LeaveTime = reader.GetDateTime("leave_time");
                destination.CostEstimate = reader.GetInt64("cost_estimate");
                destination.Note = reader.GetString("note");
                destinations.Add(destination.Id, destination);
            }
            return destinations;
        }
        public ScheduleDestination GetScheduleDestinationById(int id)
        {
            DAO dao = DAO.Instance;
            string sql = "Select * from schedules where sd_id = @id";
            MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@id", id) };
            MySqlDataReader reader = dao.ExecuteQuery(sql, parameters);
            ScheduleDestination destination = new ScheduleDestination();
            if (reader.Read())
            {
                destination.Id = reader.GetInt32("sd_id");
                destination.DestinationId = reader.GetInt32("destination_id");
                destination.ArrivalTime = reader.GetDateTime("arrival_time");
                destination.LeaveTime = reader.GetDateTime("leave_time");
                destination.CostEstimate = reader.GetInt64("cost_estimate");
                destination.Note = reader.GetString("note");
            }
            return destination;
        }

        public int UpdateScheduleDestination(ScheduleDestination destination)
        {
            DAO dao = DAO.Instance;
            string sql = "Update schedules set arrival_time = @arrivalTime, leave_time = @leaveTime, " +
                "cost_estimate = @costEstimate, note = @note where sd_id = @id";
            MySqlParameter[] parameters = new MySqlParameter[5];
            parameters[0] = new MySqlParameter("@arrivalTime", destination.ArrivalTime);
            parameters[1] = new MySqlParameter("@leaveTime", destination.LeaveTime);
            parameters[2] = new MySqlParameter("@costEstimate", destination.CostEstimate);
            parameters[3] = new MySqlParameter("@note", destination.Note);
            parameters[4] = new MySqlParameter("@id", destination.Id);
            return dao.ExecuteNonQuery(sql, parameters);
        }
        public int DeleteDestination(int id)
        {
            DAO dao = DAO.Instance;
            string sql = "Delete from schedules where sd_id = @id";
            MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@id", id) };
            return dao.ExecuteNonQuery(sql, parameters);
        }
    }
}
