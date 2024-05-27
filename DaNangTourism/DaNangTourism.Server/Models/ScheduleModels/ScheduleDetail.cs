using MySqlConnector;

namespace DaNangTourism.Server.Models.ScheduleModels
{
    public class ScheduleDetail
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public ScheduleStatus Status { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateOnly StartDate { get; set; }
        public int TotalDays { get; set; }
        public double TotalBudget { get; set; }
        public DateTime UpdateAt { get; set; }
        public string Creator { get; set; }
        public bool isPublic { get; set; }
        public List<ScheduleDay> Days { get; set; }
    }

    public class ScheduleDay
    {
        public DateOnly Date { get; set; }
        public List<DestinationOfDay> Destinations { get; set; }
    }
    public class DestinationOfDay
    {
        public int Id { get; set; }
        public int desId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public TimeOnly ArrivalTime { get; set; }
        public TimeOnly LeaveTime { get; set; }
        public double Budget { get; set; } 
        public string Note { get; set; }
    }
}
