namespace DaNangTourism.Models
{
    public class ScheduleDestination
    {
        private int _id;
        private int _destinationId;
        private DateTime _arrivalTime;
        private DateTime _leaveTime;
        private long _costEstimate;
        private string? _note;
        public int Id { get { return _id; } set { _id = value; } }
        public int DestinationId { get { return _destinationId; } set { _destinationId = value; } }
        public DateTime ArrivalTime { get { return _arrivalTime; } set { _arrivalTime = value; } }
        public DateTime LeaveTime { get { return _leaveTime; } set { _leaveTime = value; } }
        public long CostEstimate { get { return _costEstimate; } set { _costEstimate = value; } }
        public string? Note { get { return _note; } set { } }
        public ScheduleDestination() { }
        public ScheduleDestination(int id, int destinationId, DateTime arrivalTime, DateTime leaveTime, long costEstimate, string note)
        {
            _id = id;
            _destinationId = destinationId;
            _arrivalTime = arrivalTime;
            _leaveTime = leaveTime;
            _costEstimate = costEstimate;
            _note = note;
        }
    }
}
