using MySqlConnector;
using DaNangTourism.Server.Models;

namespace DaNangTourism.Server.DAL
{
    public class ReviewDAO
    {
        private DAO _dao;
        private static ReviewDAO _instance;
        public static ReviewDAO Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ReviewDAO(DAO.Instance);
                }
                return _instance;
            }
            private set { }
        }
        private ReviewDAO(DAO dao)
        {
            _dao = dao;
        }
        public Dictionary<int, Review> GetAllReviews()
        {
            string sql = "Select * from reviews";
            MySqlDataReader reader = _dao.ExecuteQuery(sql, null);
            Dictionary<int, Review> reviews = new Dictionary<int, Review>();
            _dao.OpenConnection();
            while (reader.Read())
            {
                Review review = new Review(reader);
                reviews.Add(review.Id, review);
            }
            _dao.CloseConnection();
            return reviews;
        }
        public Dictionary<int, Review> GetReviewsByDesId(int destinationId)
        {
            string sql = "Select * from reviews where destination_id = @destinationId;";
            MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@destinationId", destinationId) };
            MySqlDataReader reader = _dao.ExecuteQuery(sql, parameters);
            Dictionary<int, Review> reviews = new Dictionary<int, Review>();
            _dao.OpenConnection();
            while(reader.Read())
            {
                Review review = new Review(reader);
                reviews.Add(review.Id, review);
            }
            _dao.CloseConnection();
            return reviews;
        }
        public Review? GetReviewById(int id)
        {
            string sql = "Select * from reviews where review_id = @id";
            MySqlParameter[] parameters = [new MySqlParameter("@id", id) ];
            _dao.OpenConnection();
            MySqlDataReader reader = _dao.ExecuteQuery(sql, parameters);
            if (reader.Read())
            {
                Review review = new Review(reader);
                _dao.CloseConnection();
                return review;
            }
            _dao.CloseConnection();
            return null;
        }
        public int AddReview(int userId, int destinationId, Review review)
        {
            string sql = "Insert into reviews(user_id, destination_id, content, star) values (@userId, @destinationID, @content, @star)";
            MySqlParameter[] parameters = new MySqlParameter[4];
            parameters[0] = new MySqlParameter("@userId", userId);
            parameters[1] = new MySqlParameter("@destinationId", destinationId);
            parameters[2] = new MySqlParameter("@content", review.Content);
            parameters[3] = new MySqlParameter("@star", review.Star);
            _dao.OpenConnection();
            int result = _dao.ExecuteNonQuery(sql, parameters);
            _dao.CloseConnection();
            return result;
        }
        public int UpdateReview(Review review)
        {
            string sql = "Update reviews set content = @content, star = @star where review_id = @review_id";
            MySqlParameter[] parameters = new MySqlParameter[3];
            parameters[0] = new MySqlParameter("@content", review.Content);
            parameters[1] = new MySqlParameter("@star", review.Star);
            parameters[2] = new MySqlParameter("@review_id", review.Id);
            _dao.OpenConnection();
            int result = _dao.ExecuteNonQuery(sql, parameters);
            _dao.CloseConnection();
            return result;
        }
        public int DeleteReview(int id)
        {
            string sql = "Delete from reviews where review_id = @id";
            MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@id", id) };
            _dao.OpenConnection();
            int result = _dao.ExecuteNonQuery(sql, parameters);
            _dao.CloseConnection();
            return result;
        }
    }
}
