namespace DaNangTourism.Models
{
    public class Schedule
    {
        public string scheduleID {  get; set; }
        public string scheduleName { get; set; }
        public DateTime creationTime { get; set; }
        public string describe {  get; set; }
        public ScheduleState state { get; set; }
        public bool isPublic { get; set; }

        public Schedule()
        {

        }
    }
}
