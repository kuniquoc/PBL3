using DaNangTourism.Server.DAL;
using DaNangTourism.Server.Models;
using DaNangTourism.Server.Models.ReviewModels;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using MySqlConnector;
using System.Text;

namespace DaNangTourism.Server.Services
{
    public interface IReviewService
    {
        DestinationReviews GetReviewsByDestinationId(int destinationId, ReviewFilter reviewFilter);
        int AddReview(int userId, InputReviewModel review);
        void DeleteReview(int reviewId);
    }
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IDestinationRepository _destinationRepository;
        public ReviewService(IReviewRepository reviewRepository, IDestinationRepository destinationRepository)
        {
            _reviewRepository = reviewRepository;
            _destinationRepository = destinationRepository;
        }

        /// <summary>
        /// Get Reviews by DestinationId
        /// </summary>
        /// <param name="destinationId"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortType"></param>
        /// <returns></returns>
        public DestinationReviews GetReviewsByDestinationId(int destinationId, ReviewFilter reviewFilter)
        {
            DestinationReviews destinationReviews = new DestinationReviews();

            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT review_id AS Id, Users.full_name AS Author, Users.avatar_url AS Avatar, rating AS Rating, comment AS Comment, Reviews.created_at AS Created_At" +
                " FROM Reviews INNER JOIN Users ON Users.user_id = Reviews.user_id WHERE destination_id = @destinationId");

            List<MySqlParameter> parameters = new List<MySqlParameter>();
            parameters.Add(new MySqlParameter("@destinationId", destinationId));

            // xử lý order by
            if (!string.IsNullOrEmpty(reviewFilter.SortBy))
            {
                sql.Append(" ORDER BY " + reviewFilter.SortBy);
            }
            else
            {
                sql.Append(" ORDER BY created_at");
            }

            if (!string.IsNullOrEmpty(reviewFilter.SortType))
            {
                sql.Append(" " + reviewFilter.SortType);
            }
            else
            {
                sql.Append(" desc");
            }

            // Lấy tổng kết quả có được
            StringBuilder countSql = new StringBuilder();
            countSql.Append("SELECT COUNT(*) FROM (" + sql + ") AS subquery");
            destinationReviews.Total = _reviewRepository.GetReviewCount(countSql.ToString(), parameters.ToArray());

            // xử lý limit
            sql.Append(" LIMIT @limit OFFSET @offset");
            destinationReviews.Limit = reviewFilter.Limit;
            parameters.Add(new MySqlParameter("@limit", reviewFilter.Limit));
            destinationReviews.Page = reviewFilter.Page;
            parameters.Add(new MySqlParameter("@offset", (reviewFilter.Page - 1) * reviewFilter.Limit));

            destinationReviews.Items = _reviewRepository.GetReviewsByDestinationId(sql.ToString(), parameters.ToArray()).ToList();

            return destinationReviews;
        }


        /// <summary>
        /// Add review to DataBase
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="destinationId"></param>
        /// <param name="review"></param>
        /// <returns> Return id of added review</returns>
        public int AddReview(int userId, InputReviewModel review)
        {
            return _reviewRepository.AddReview(userId, review);
        }

        public void DeleteReview(int reviewId)
        {
            _reviewRepository.DeleteReview(reviewId);
        }
    }
}
