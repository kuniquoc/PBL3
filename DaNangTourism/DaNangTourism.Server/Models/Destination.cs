using Microsoft.AspNetCore.Mvc.ViewEngines;
using MySqlConnector;

namespace DaNangTourism.Server.Models
{
    public class Destination
    {
        private int _id;
        private string? _name;
        private string? _address;
        private TimeOnly _openTime;
        private TimeOnly _closeTime;
        private DayOfWeek _openDay;
        private string? _htmlText;
        private string[] _imgURL;
        private float _rating;
        private Dictionary<int, Review>? _reviews;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string? Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string? Address
        {
            get { return _address; }
            set { _address = value; }
        }
        public TimeOnly OpenTime
        {
            get { return _openTime; }
            set { _openTime = value; }
        }
        public TimeOnly CloseTime
        {
            get { return _closeTime; }
            set { _closeTime = value; }
        }
        public DayOfWeek OpenDay
        {
            get { return _openDay; }
            set { _openDay = value; }
        }
        public string? HtmlText
        {
            get { return _htmlText; }
            set { _htmlText = value; }
        }
        public string[] ImgURL
        {
            get { return _imgURL; }
            set { _imgURL = value; }
        }
        public float Rating
        {
            get { return _rating; }
            set { _rating = value; }
        }
        public Dictionary<int, Review>? Reviews
        {
            get { return _reviews; }
            set { _reviews = value; }
        }
        public Destination()
        {
            _imgURL = new string[0];
            _reviews = new Dictionary<int, Review>();
        }
        public Destination(MySqlDataReader reader)
        {
            _id = reader.GetInt32("destination_id");
            _name = reader.GetString("destination_name");
            _address = reader.GetString("destination_address");
            _openTime = TimeOnly.Parse(reader.GetString("open_time"));
            _closeTime = TimeOnly.Parse(reader.GetString("close_time"));
            _openDay = Enum.Parse<DayOfWeek>(reader.GetString("destination_address"));
            _htmlText = reader.GetString("destination_address");
            _imgURL = reader.GetString("destination_address").Split(';');
            _rating = reader.GetFloat("rating");
            _reviews = new Dictionary<int, Review>();
        }
        public void AddReview(Review review)
        {
            if (_reviews == null) _reviews = new Dictionary<int, Review>();
            _reviews.Add(review.Id, review);
        }
        public void AddRangeReview(Review[] reviews)
        {
            if (_reviews == null) _reviews = new Dictionary<int, Review>();
            foreach (Review review in reviews)
            {
                _reviews.Add(review.Id, review);
            }
        }
        public bool UpdateReview(Review review)
        {
            if (_reviews == null) _reviews = new Dictionary<int, Review>();
            if (_reviews.ContainsKey(review.Id))
            {
                Review changeReview = _reviews[review.Id];
                changeReview.Content = review.Content;
                changeReview.Star = review.Star;
                return true;
            }
            return false;
        }
        public void DeleteReview(int reviewId)
        {
            if (_reviews == null) _reviews = new Dictionary<int, Review>();
            if (_reviews.ContainsKey(reviewId))
            {
                _reviews.Remove(reviewId);
            }    
                
        }
    }
}
