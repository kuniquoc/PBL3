namespace DaNangTourism.Server.Models
{
    public class AddReviewRequest
    {
        public int UserId { get; set; }
        public Review Review { get; set; }
    }
}
