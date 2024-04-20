namespace DaNangTourism.Models
{
    public class ScheduleDestination
    {
        public string SDID { get; set; }
        public string destinationID { get; set; }
        public string scheduleID { get; set; }
        public DateTime arrivalTime { get; set; }
        public DateTime leaveTime { get; set; }
        public int costEstimate { get; set; }
        public string note { get; set; }

        public ScheduleDestination()
        {

        }

    }
}

