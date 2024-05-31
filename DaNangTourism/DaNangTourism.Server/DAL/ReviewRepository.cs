using MySqlConnector;
using DaNangTourism.Server.Models.ReviewModels;

namespace DaNangTourism.Server.DAL
{
    public interface IReviewRepository
    {
        Dictionary<int, int> GetReviewsCountByDesIdGroupedByRating(int destinationId);
        IEnumerable<DestinationReview> GetReviewsByDestinationId(string sql, params MySqlParameter[] parameters);
        int GetReviewCount(string sql, params MySqlParameter[] parameters);
        int AddReview(int userId, InputReviewModel review);
        int DeleteReview(int userId);
    }
    public class ReviewRepository : IReviewRepository
    {
        private string _connectionString;
        public ReviewRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Get counts of reviews by rating for a specific destination.
        /// </summary>
        /// <param name="destinationId">The ID of the destination.</param>
        /// <returns>A dictionary with ratings as keys and counts as values.</returns>
        public Dictionary<int, int> GetReviewsCountByDesIdGroupedByRating(int destinationId)
        {
            // SQL query to count reviews grouped by rating
            string sql = "SELECT Rating, COUNT(*) AS Count FROM Reviews WHERE DestinationId = @destinationId GROUP BY Rating";
            MySqlParameter parameter = new MySqlParameter("@destinationId", destinationId);

            // Create a dictionary to store the results
            Dictionary<int, int> reviewCountsByRating = new Dictionary<int, int>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add(parameter);

                    // Execute the query and read the results
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int rating = reader.GetInt32("Rating");
                            int count = reader.GetInt32("Count");

                            // Add the count to the dictionary
                            reviewCountsByRating[rating] = count;
                        }
                    }
                }
            }

            return reviewCountsByRating;
        }

        /// <summary>
        /// Get reviews by destination ID.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<DestinationReview> GetReviewsByDestinationId(string sql, params MySqlParameter[] parameters)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddRange(parameters);
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

        /// <summary>
        /// Get review count
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int GetReviewCount(string sql, params MySqlParameter[] parameters)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddRange(parameters);
                    return Convert.ToInt32(command.ExecuteScalar());
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
        public int AddReview(int userId, InputReviewModel review)
        {
            string sql = "Insert into Reviews(UserId, DestinationId, Rating, Comment, Created_At) values (@userId, @destinationId, @rating, @comment);" +
                "SELECT LAST_INSERT_ID();";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@userId", userId),
                new MySqlParameter("@destinationId", review.DestinationId),
                new MySqlParameter("@rating", review.Rating),
                new MySqlParameter("@comment", review.Comment)
            };
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddRange(parameters);
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// Delete review by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteReview(int id)
        {
            string sql = "DELETE FROM Reviews WHERE ReviewId = @id";
            MySqlParameter parameter = new MySqlParameter("@id", id);
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add(parameter);
                    return command.ExecuteNonQuery();
                }
            }
        }

    }
}
