namespace DaNangTourism.Server.Models.ReviewModels
{
    public class InputReviewModel
    {
        public int DestinationId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = "";
    }
}
