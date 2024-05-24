using DaNangTourism.Server.DAL;
using DaNangTourism.Server.Models;
using DaNangTourism.Server.Middleware;
using Microsoft.AspNetCore.Components.Web;
using System.Collections.Generic;
using MySqlConnector;
using System.Text;
namespace DaNangTourism.Server.Service
{
    public interface IDestinationService
    {
        IEnumerable<HomeDestination> GetHomeDestinations();
        IEnumerable<ListDestination> GetListDestinations(int userId, DestinationFilter destinationFilter);
        DestinationDetail? GetDestinationDetail(int id);
        IEnumerable<DestinationReview> GetDestinationReviews(int destinationId);
        int AddReview(int userId, int destinationId, Review review);
        void UpdateFavDes(int userId, int favDesId, bool favorite);
        IEnumerable<HomeDestination> GetRandomDestinations(IQueryCollection query);
        AdminDestinations GetDestinationElements(IQueryCollection query);
        IEnumerable<DestinationReview> GetReviewsByDesId(int destinationId, IQueryCollection query);
        int AddDestination(InputDestinationModel destination);
    }
    public class DestinationService : IDestinationService
    {
        private readonly IDestinationRepository _destinationRepository;
        private readonly IFavoriteDestinationRepository _favoriteDestinationRepository;
        private readonly IReviewRepository _reviewRepository;
        public DestinationService(IDestinationRepository destinationRepository, IFavoriteDestinationRepository favoriteDestinationRepository, IReviewRepository reviewRepository)
        {
            _destinationRepository = destinationRepository;
            _favoriteDestinationRepository = favoriteDestinationRepository;
            _reviewRepository = reviewRepository;
        }
        public IEnumerable<HomeDestination> GetHomeDestinations()
        {
            return _destinationRepository.GetNewestDestinations();
        }


        

        // hàm khử điều kiện lọc destinationFilter 
        private DestinationFilter destinationationSanitization(DestinationFilter destinationFilter)
        {
            destinationFilter.Search = DataSanitization.RemoveSpecialCharacters(destinationFilter.Search);
            destinationFilter.Location = DataSanitization.RemoveSpecialCharacters(destinationFilter.Location);
            return destinationFilter;
        }
        public IEnumerable<ListDestination> GetListDestinations(int userId, DestinationFilter destinationFilter)
        {
            // khử điều kiện lọc
            destinationFilter = destinationationSanitization(destinationFilter);

            // xử lý query để có điều kiện lọc
            StringBuilder filter = new StringBuilder();
            List<MySqlParameter> parameters = new List<MySqlParameter>();

            // xử lý favorite Des
            if (destinationFilter.IsFavorite != null)
            {
                filter.Append(" LEFT JOIN FavoriteDestinations ON FavoriteDestinations.DestinationId = Destinations.Id");
            }

            // xử lý các bộ lọc dùng where
            if (!string.IsNullOrEmpty(destinationFilter.Search))
            {
                filter.Append(" WHERE Name = @search");
                parameters.Add(new MySqlParameter("@search", destinationFilter.Search));
            }

            if (!string.IsNullOrEmpty(destinationFilter.Location))
            {
                if (filter.ToString().Contains("WHERE"))
                {
                    filter.Append(" AND Location LIKE @location");
                }
                else
                {
                    filter.Append(" WHERE Location LIKE @location");
                }
                parameters.Add(new MySqlParameter("@location", "%" + destinationFilter.Location + "%"));
            }

            if (destinationFilter.CostFrom != -1)
            {
                if (filter.ToString().Contains("WHERE"))
                {
                    filter.Append(" AND Cost >= @costFrom");
                }
                else
                {
                    filter.Append(" WHERE Cost >= @costFrom");
                }
                parameters.Add(new MySqlParameter("@costFrom", destinationFilter.CostFrom));
            }

            if (destinationFilter.CostTo != -1)
            {
                if (filter.ToString().Contains("WHERE"))
                {
                    filter.Append(" AND Cost <= @costTo");
                }
                else
                {
                    filter.Append(" WHERE Cost <= @costTo");
                }
                parameters.Add(new MySqlParameter("@costTo", destinationFilter.CostTo));
            }

            if (destinationFilter.RatingFrom != -1)
            {
                if (filter.ToString().Contains("WHERE"))
                {
                    filter.Append(" AND Rating >= @ratingFrom");
                }
                else
                {
                    filter.Append(" WHERE Rating >= @ratingFrom");
                }
                parameters.Add(new MySqlParameter("@ratingFrom", destinationFilter.RatingFrom));
            }

            if (destinationFilter.RatingTo != -1)
            {
                if (filter.ToString().Contains("WHERE"))
                {
                    filter.Append(" AND Rating <= @ratingTo");
                }
                else
                {
                    filter.Append(" WHERE Rating <= @ratingTo");
                }
                parameters.Add(new MySqlParameter("@ratingTo", destinationFilter.RatingTo));
            }

            if (destinationFilter.IsFavorite != null)
            {
                if (filter.ToString().Contains("WHERE"))
                {
                    filter.Append(" AND UserId = @userId");
                }
                else
                {
                    filter.Append(" WHERE UserId = @userId");
                }
                parameters.Add(new MySqlParameter("@userId", userId));
            }

            // xử lý order by
            if (!string.IsNullOrEmpty(destinationFilter.SortBy))
            {
                filter.Append(" ORDER BY " + destinationFilter.SortBy);
            }
            else
            {
                filter.Append(" ORDER BY created_at");
            }

            if (!string.IsNullOrEmpty(destinationFilter.SortType))
            {
                filter.Append(" " + destinationFilter.SortType);
            }
            else
            {
                filter.Append(" ASC");
            }


            return _destinationRepository.GetListDestination(filter.ToString(), parameters);
        }

        public DestinationDetail? GetDestinationDetail(int id)
        {
            DestinationDetail? destinationDetail = _destinationRepository.GetDestinationById(id);
            if (destinationDetail == null) return null;
            // xử lý để nhận thông tin cho GeneralReview
            destinationDetail.GeneralReview.TotalReview = _reviewRepository.GetReviewsCountByDesId(id);
            for (int i = 5; i >= 1; i--)
            {
                int countOfRating = _reviewRepository.GetReviewsCountByDesId(id, i);
                destinationDetail.GeneralReview.AddRatingPercent(i, countOfRating);
            }    
            return destinationDetail;
        }

        public IEnumerable<DestinationReview> GetDestinationReviews(int destinationId)
        {
            return null;
        }

        /// <summary>
        /// Add review to DataBase
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="destinationId"></param>
        /// <param name="review"></param>
        /// <returns> Return id of added review</returns>
        public int AddReview(int userId, int destinationId, Review review)
        {
            return _reviewRepository.AddReview(userId, destinationId, review);
        }

        public void UpdateFavDes(int userId, int favDesId, bool favorite)
        {
            if (favorite)
            {
                _favoriteDestinationRepository.AddFavDes(userId, favDesId);
            }
            else
            {
                _favoriteDestinationRepository.DeleteFavDes(userId, favDesId);
            }
        }
        public IEnumerable<HomeDestination> GetRandomDestinations(IQueryCollection query)
        {
            int limit = 3;
            if (query.ContainsKey("limit"))
            {
                string? limitStr = query["limit"];
                // Kiểm tra null trước khi Parse
                if (!string.IsNullOrEmpty(limitStr))
                {
                    limit = Int32.Parse(limitStr);
                }
            }    
              
            return _destinationRepository.GetRandomDestinations(limit);
        }

        public AdminDestinations GetDestinationElements(IQueryCollection query)
        {
            AdminDestinations adminDestinations = new();
            //mặc định limit là 12
            adminDestinations.Limit = 12;
            //mặc định page là 1
            adminDestinations.Page = 1;

            //Lấy item của adminDestinations
            string sql = "Select d.DestinationId, d.Name, d.Address, d.Rating, d.Created_At, " +
                "(Select count(*) from Reviews r where r.DestinationId = d.DestinationId) as Review, " +
                "(Select count(*) from FavoriteDestinations where f.DestinationId = d.DestinationId) as Favourite, " +
                "d.Created_At from Destination d";

            // điều kiện lọc (nếu có)
            string filter = "";
            //xử lý lọc dùng where
            if (query.ContainsKey("search"))
            {
                filter += " where d.Name = '" + query["search"] + "'";
            }

            // xử lý bộ lọc dùng order by
            filter += " order by ";
            if (query.ContainsKey("sortBy"))
            {
                if (query.ContainsKey("created_at") || query.ContainsKey("rating"))
                {
                    filter += "d.";
                }
                filter += query["sortBy"];
            }
            else filter += "d.created_at";
            if (query.ContainsKey("sortType"))
            {
                filter += " " + query["sortType"];
            }

            //xử lý trang hiển thị và số lượng tương ứng
            filter += " limit";
                
            // kiểm tra limit
            if (query.ContainsKey("limit"))
            {
                string? limitStr = query["limit"];
                if (!String.IsNullOrEmpty(limitStr))
                {
                    adminDestinations.Limit = Int32.Parse(limitStr);
                }
            }
            filter += " " + adminDestinations.Limit;

            // kiểm tra page
                
            if (query.ContainsKey("page"))
            {
                string? pageStr = query["page"];
                if (!String.IsNullOrEmpty(pageStr))
                {
                    adminDestinations.Page = Int32.Parse(pageStr);
                }
            }
            filter += "offset " + (adminDestinations.Page - 1) * adminDestinations.Limit;                
            
            adminDestinations.Items = _destinationRepository.GetDestinationElements(sql + filter).ToList();
            //Lấy tổng kết quả có được
            string countMySql = (sql + filter).Insert(7, "count(*) ");
            countMySql = countMySql.Remove(countMySql.IndexOf("limit"));
            adminDestinations.Total = _destinationRepository.GetDestinationCount(countMySql);

            return adminDestinations;
        }

        //hụt phần user
        public IEnumerable<DestinationReview> GetReviewsByDesId(int destinationId, IQueryCollection query)
        {
            //string filter;
            if (query.ContainsKey("sortBy"))
            {

            }
            return null;
        }
        public int AddDestination (InputDestinationModel destination)
        {
            return _destinationRepository.AddDestination(destination);
        }
    }
}
