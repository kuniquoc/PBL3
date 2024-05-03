namespace DaNangTourism.Server.Models
{
    public enum ScheduleStatus
    {
        planning = 0,
        processing = 1,
        completed = 2,
        cancelled = 3,
    }
    public class Schedule
    {
        private int _id;
        private int _userId;
        private string? _name;
        private string? _describe;
        private ScheduleStatus _status;
        private bool _isPublic;
        private Dictionary<int, ScheduleDestination> _destinations;
        public int Id { get { return _id; } set { _id = value; } }
        public int UserId { get { return _userId; } set { _userId = value; } }
        public string? Name { get { return _name; } set { _name = value; } }
        public string? Describe { get { return _describe; } set { _describe = value; } }
        public ScheduleStatus Status { get { return _status; } set { _status = value; } }
        public bool IsPublic { get { return _isPublic; } set { _isPublic = value; } }
        public Dictionary<int, ScheduleDestination> Destinations
        {
            get { return _destinations; }
            set { _destinations = value; }
        }
        public Schedule()
        {
            _destinations = new Dictionary<int, ScheduleDestination>();
        }
        public Schedule(int id, int userId, string name, string describe, ScheduleStatus status, bool isPublic, Dictionary<int, ScheduleDestination> destinations)
        {
            _id = id;
            _userId = userId;
            _name = name;
            _describe = describe;
            _status = status;
            _isPublic = isPublic;
            _destinations = destinations;
        }
        public void AddDestination(ScheduleDestination destination)
        {
            _destinations.Add(destination.Id, destination);
        }
        public void AddRangeDestination(ScheduleDestination[] destinations)
        {
            foreach (ScheduleDestination destination in destinations)
            {
                _destinations.Add(destination.Id, destination);
            }
        }
        public void UpdateDestination(ScheduleDestination destination)
        {
            ScheduleDestination changeDestination = _destinations[destination.Id];
            changeDestination.ArrivalTime = destination.ArrivalTime;
            changeDestination.LeaveTime = destination.LeaveTime;
            changeDestination.CostEstimate = destination.CostEstimate;
            changeDestination.Note = destination.Note;
        }
        public void DeleteReview(int destinationId)
        {
            _destinations.Remove(destinationId);
        }
    }
}
