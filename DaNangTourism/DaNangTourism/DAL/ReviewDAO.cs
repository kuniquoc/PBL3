using DaNangTourism.Models;
using MySqlConnector;

namespace DaNangTourism.DAL
{
    public class ReviewDAO
    {
        public Dictionary<int, Review> GetAllReviews()
        {
            DAO dao = new DAO();
            string sql = "Select * from reviews";
            MySqlDataReader reader = dao.ExecuteQuery(sql, null);
            Dictionary<int, Review> reviews = new Dictionary<int, Review>();
            while (reader.Read())
            {
                Review review = new Review();
                review.Id = reader.GetInt32("review_id");
                review.Content = reader.GetString("content");
                review.Star = reader.GetInt32("star");
                reviews.Add(review.Id, review);
            }
            return reviews;
        }
        public Dictionary<int, Review> GetReviews(int destinationId)
        {
            DAO dao = new DAO();
            string sql = "Select * from reviews where destination_id = @destinationId;";
            MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@destinationId", destinationId) };
            MySqlDataReader reader = dao.ExecuteQuery(sql, parameters);
            Dictionary<int, Review> reviews = new Dictionary<int, Review>();
            while(reader.Read())
            {
                Review review = new Review();
                review.Id = reader.GetInt32("review_id");
                review.Content = reader.GetString("content");
                review.Star = reader.GetInt32("star");
                reviews.Add(review.Id, review);
            }
            return reviews;
        }
        public int UpdateReview(Review review)
        {
            DAO dao = new DAO();
            string sql = "Update reviews set content = @content, star = @star where review_id = @review_id";
            MySqlParameter[] parameters = new MySqlParameter[3];
            parameters[0] = new MySqlParameter("@content", review.Content);
            parameters[1] = new MySqlParameter("@star", review.Star);
            parameters[2] = new MySqlParameter("@review_id", review.Id);
            return dao.ExecuteNonQuery(sql, parameters);
        }
        public int DeleteReview(int id)
        {
            DAO dao = new DAO();
            string sql = "Delete from reviews where review_id = @id";
            MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@id", id) };
            return dao.ExecuteNonQuery(sql, parameters);
        }
    }
}
