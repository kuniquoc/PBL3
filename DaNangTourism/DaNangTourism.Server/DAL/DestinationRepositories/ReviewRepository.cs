using MySqlConnector;
using DaNangTourism.Server.Models;

namespace DaNangTourism.Server.DAL
{
    public interface IReviewRepository
    {
        int GetReviewsCountByDesId(int destinationId, int rating = 0);
        int AddReview(int userId, int destinationId, Review review);
        IEnumerable<DestinationReview> GetDestinationReviews(int destinationId, string filter = "");
    }
    public class ReviewRepository : IReviewRepository
    {
        private string _connectionString;
        public ReviewRepository(string connectionString)
        {
            _connectionString = connectionString;
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
            using (var connection = new MySqlConnection(_connectionString)) {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add(parameter);
                    return (int)command.ExecuteScalar();
                }
            }
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
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddRange(parameters);
                    command.ExecuteNonQuery();
                    var id = (int) command.ExecuteScalar();
                    return id;
                }
            }
        }

        public IEnumerable<DestinationReview> GetDestinationReviews(int destinationId, string filter = "")
        {
            string sql = "Select * from Reviews where DestinationId = @destinationId";
            MySqlParameter parameter = new MySqlParameter("@destinationId", destinationId);
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add(parameter);
                    using (var reader = command.ExecuteReader())
                    {
                        var reviews = new List<DestinationReview>();
                        while (reader.Read())
                        {
                            DestinationReview review = new DestinationReview(reader);
                            reviews.Add(review);
                        }
                        return reviews;
                    }
                }
            }
        }
        
    }
}
