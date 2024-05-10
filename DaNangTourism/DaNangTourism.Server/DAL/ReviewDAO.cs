﻿using MySqlConnector;
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
            string sql = "Select * from reviews where destination_id = @destinationId;";
            MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@destinationId", destinationId) };
            MySqlDataReader reader = _dao.ExecuteQuery(sql, parameters);
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
            string sql = "Update reviews set content = @content, star = @star where review_id = @review_id";
            MySqlParameter[] parameters = new MySqlParameter[3];
            parameters[0] = new MySqlParameter("@content", review.Content);
            parameters[1] = new MySqlParameter("@star", review.Star);
            parameters[2] = new MySqlParameter("@review_id", review.Id);
            return _dao.ExecuteNonQuery(sql, parameters);
        }
        public int DeleteReview(int id)
        {
            string sql = "Delete from reviews where review_id = @id";
            MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@id", id) };
            return _dao.ExecuteNonQuery(sql, parameters);
        }
    }
}
