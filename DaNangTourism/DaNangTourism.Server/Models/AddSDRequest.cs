namespace DaNangTourism.Server.Models
{
    public class AddSDRequest
    {
        public int ScheduleId {  get; set; }   
        public int DestinationId { get; set; }
        public ScheduleDestination SD { get; set; }
    }
}
