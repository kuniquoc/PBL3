namespace DaNangTourism.Server.Models
{
    public class Blog
    {
        private int _id;
        private string? _title;
        private string? _content;
        private string? _image;
        private DateTime _date;
        private int _view;
        private int _like;
        private int _accountId;
        public int Id { get { return _id; } set { _id = value; } }
        public string? Title { get { return _title; } set { _title = value; } }
        public string? Content { get { return _content; } set { _content = value; } }
        public string? Image { get { return _image; } set { _image = value; } }
        public DateTime Date { get { return _date; } set { _date = value; } }
        public int View { get { return _view; } set { _view = value; } }
        public int Like { get { return _like; } set { _like = value; } }
        public int AccountId { get { return _accountId; } set { _accountId = value; } }
        public Blog() { }
        public Blog(int id, string title, string content, string image, DateTime date, int view, int like, int accountId)
        {
            _id = id;
            _title = title;
            _content = content;
            _image = image;
            _date = date;
            _view = view;
            _like = like;
            _accountId = accountId;
        }

    }
}
