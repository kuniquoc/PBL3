namespace DaNangTourism.Server.Models
{
    public class Destination
    {
        private int _id;
        private string _name;
        private string _address;
        private TimeOnly _openTime;
        private TimeOnly _closeTime;
        private DayOfWeek _openDay;
        private string _htmlText;
        private string[] _imgURL;
        private float _rating;
        private Dictionary<int, Review> _reviews;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string Address
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
        public string HtmlText
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
        public Dictionary<int, Review> Reviews
        {
            get { return _reviews; }
            set { _reviews = value; }
        }
        public Destination() { }
        public Destination(int id, string name, string address, TimeOnly openTime, TimeOnly closeTime, DayOfWeek openDay, string htmlText, string[] imgURL, float rating)
        {
            _id = id;
            _name = name;
            _address = address;
            _openTime = openTime;
            _closeTime = closeTime;
            _openDay = openDay;
            _htmlText = htmlText;
            _imgURL = imgURL;
            _rating = rating;
        }
        public void AddReview(Review review)
        {
            _reviews.Add(review.Id, review);
        }
        public void AddRangeReview(Review[] reviews)
        {
            foreach (Review review in reviews)
            {
                _reviews.Add(review.Id, review);
            }
        }
        public void UpdateReview(Review review)
        {
            Review changeReview = _reviews[review.Id];
            changeReview.Content = review.Content;
            changeReview.Star = review.Star;
        }
        public void DeleteReview(int reviewId)
        {
            _reviews.Remove(reviewId);
        }
    }
}
