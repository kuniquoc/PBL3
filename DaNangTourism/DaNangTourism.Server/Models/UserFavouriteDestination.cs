namespace DaNangTourism.Server.Models
{
    public class UserFavouriteDestination
    {
        private int _id;
        private int _userId;
        private int _destinationId;
        public int Id { get { return _id; } set { _id = value; } }
        public int UserId { get { return _userId; } set { _userId = value; } }
        public int DestinationId { get { return _destinationId; } set { _destinationId = value; } }
        public UserFavouriteDestination() { }
        public UserFavouriteDestination(int id, int userId, int destinationId)
        { _id = id; _userId = userId; _destinationId = destinationId; }

    }
}
