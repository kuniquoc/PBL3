using MySqlConnector;

namespace DaNangTourism.Server.Models
{
    public class ScheduleDestination
    {
        public int Id { get; set; }
        public int DestinationId { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime LeaveTime { get; set; }
        public long CostEstimate { get; set; }
        public string? Note { get; set; }
        public ScheduleDestination() { }
        public ScheduleDestination(MySqlDataReader reader)
        {
            Id = reader.GetInt32("sd_id");
            DestinationId = reader.GetInt32("destination_id");
            ArrivalTime = reader.GetDateTime("arrival_time");
            LeaveTime = reader.GetDateTime("leave_time");
            CostEstimate = reader.GetInt64("cost_estimate");
            Note = reader.GetString("note");
        }
    }
}
