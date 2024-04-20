namespace DaNangTourism.Models
{
    public class Destination
    {
        public string destinationID {  get; set; }
        public string name { get; set; }   
        public string address { get; set; }
        public string describe { get; set; }
        public DateTime _openTime { get; set; }
        public DateTime _closeTime { get; set; }
        public DateTime _openDayTime { get; set; }
        public string[] _imgURL { get; set; }
        public float _rating { get; set; }

        public Destination()
        {
        
        }

        // Add method

    }
}
