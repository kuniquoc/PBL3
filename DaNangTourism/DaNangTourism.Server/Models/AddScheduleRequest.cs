namespace DaNangTourism.Server.Models
{
    public class AddScheduleRequest
    {
        public int UserId { get; set; }
        public Schedule Schedule { get; set; }
    }
}
