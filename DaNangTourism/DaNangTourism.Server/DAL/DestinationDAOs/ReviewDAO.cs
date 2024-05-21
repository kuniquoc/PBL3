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
        /// <summary>
        /// Count all or count by rating
        /// </summary>
        /// <param name="destinationId"></param>
        /// <param name="rating"></param>
        /// <returns></returns>
        public int GetReviewsCountByDesId(int destinationId, int rating = 0)
        {
            string sql = "Select count(*) from Review where DestinationId = @destinationId";
            if (rating > 0)
            {
                sql += " and Rating = " + rating;
            }    
            MySqlParameter parameter = new MySqlParameter("@destinationId", destinationId);
            int? result;
            _dao.OpenConnection();
            result = (int?) DAO.Instance.ExecuteScalar(sql, parameter);
            _dao.CloseConnection();
            if (result == null) result = 0;
            return result.Value;
        }
        /// <summary>
        /// Add review with destinationId, userId
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="destinationId"></param>
        /// <param name="review"></param>
        /// <returns>
        /// Return id of added review
        /// </returns>
        public int AddReview(int userId, int destinationId, Review review)
        {
            string sql = "Insert into Review(UserId, DestinationId, Rating, Comment, Created_At) values (@userId, @destinationId, @rating, @comment, @create_at)";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@userId", userId),
                new MySqlParameter("@destinationId", destinationId),
                new MySqlParameter("@rating", review.Rating),
                new MySqlParameter("@comment", review.Comment),
                new MySqlParameter("@create_at", review.Created_At)
            };
            _dao.OpenConnection();
            _dao.ExecuteNonQuery(sql, parameters);
            sql = "Select SCOPE_IDENTITY();";
            int id =((int?) _dao.ExecuteScalar(sql)).Value;
            _dao.CloseConnection();
            return id;
        }

        public List<DestinationReview> GetDestinationReviews(int destinationId, string filter = "")
        {
            string sql = "Select * from Reviews where DestinationId = @destinationId";
            MySqlParameter parameter = new MySqlParameter("@destinationId", destinationId);
            _dao.OpenConnection();
            MySqlDataReader reader = _dao.ExecuteQuery(sql, parameter);
            List<DestinationReview> reviews = new List<DestinationReview>();
            while (reader.Read())
            {
                DestinationReview review = new DestinationReview(reader);
                reviews.Add(review);
            }
            _dao.CloseConnection();
            return reviews;
        }
        
    }
}
