using MySqlConnector;
using DaNangTourism.Server.Models.ScheduleModels;

namespace DaNangTourism.Server.DAL
{
    public class ScheduleDestinationRepository
    {
        private readonly DAO _dao;
        private static ScheduleDestinationRepository _instance;
        public static ScheduleDestinationRepository Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ScheduleDestinationRepository(DAO.Instance);
                }
                return _instance;
            }
        }
        private ScheduleDestinationRepository(DAO dao)
        {
            _dao = dao;
        }
        public List<ScheduleDestination> GetSDsByScheduleID(int scheduleId)
        {
            string sql = "Select * from schedule_destinations where schedule_id = @scheduleId";
            MySqlParameter[] parameters = new MySqlParameter[] {new MySqlParameter("scheduleId", scheduleId)};
            MySqlDataReader reader = _dao.ExecuteQuery(sql, parameters);
            List<ScheduleDestination> sDs = new List<ScheduleDestination>();
            _dao.OpenConnection();
            while (reader.Read())
            {
                ScheduleDestination sD = new ScheduleDestination(reader);
                sDs.Add(sD);
            }
            _dao.CloseConnection();
            return sDs;
        }
        public ScheduleDestination? GetSDById(int id)
        {
            string sql = "Select * from schedule_destinations where sd_id = @id";
            MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@id", id) };
            MySqlDataReader reader = _dao.ExecuteQuery(sql, parameters);
            
            _dao.OpenConnection();
            if (reader.Read())
            {
                ScheduleDestination sD = new ScheduleDestination(reader);
                _dao.CloseConnection();
                return sD;
            }
            _dao.CloseConnection();
            return null;
        }
        public int AddSD(int scheduleId, int destinationId, ScheduleDestination sD)
        {
            string sql = "Insert into schedule_destinations(schedule_id, destination_id, arrival_time, leave_time, cost_estimate, note) values(@scheduleId, @destinationId, @arrivalTime, @leaveTime, " +
                "@costEstimate, @note)";
            MySqlParameter[] parameters = new MySqlParameter[6];
            parameters[1] = new MySqlParameter("@scheduleId", scheduleId);
            parameters[2] = new MySqlParameter("@destinationId", destinationId);
            parameters[3] = new MySqlParameter("@arrivalTime", sD.ArrivalTime);
            parameters[4] = new MySqlParameter("@leaveTime", sD.LeaveTime);
            parameters[5] = new MySqlParameter("@costEstimate", sD.CostEstimate);
            parameters[6] = new MySqlParameter("@note", sD.Note);
            
            _dao.OpenConnection();
            int result = _dao.ExecuteNonQuery(sql, parameters);
            _dao.CloseConnection();
            return result;
        }
        public int UpdateSD(ScheduleDestination sD)
        {
            string sql = "Update schedule_destinations set destination_id = @destinationId, arrival_time = @arrivalTime, leave_time = @leaveTime, " +
                "cost_estimate = @costEstimate, note = @note where sd_id = @id";
            MySqlParameter[] parameters = new MySqlParameter[6];
            parameters[0] = new MySqlParameter("@destinationId", sD.DestinationId);
            parameters[1] = new MySqlParameter("@arrivalTime", sD.ArrivalTime);
            parameters[2] = new MySqlParameter("@leaveTime", sD.LeaveTime);
            parameters[3] = new MySqlParameter("@costEstimate", sD.CostEstimate);
            parameters[4] = new MySqlParameter("@note", sD.Note);
            parameters[5] = new MySqlParameter("@id", sD.Id);
            _dao.OpenConnection();
            int result = _dao.ExecuteNonQuery(sql, parameters);
            _dao.CloseConnection();
            return result;
        }
        public int DeleteSD(int id)
        {
            string sql = "Delete from schedule_destinations where sd_id = @id";
            MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@id", id) };
            _dao.OpenConnection();
            int result = _dao.ExecuteNonQuery(sql, parameters);
            _dao.CloseConnection();
            return result;
        }
    }
}
