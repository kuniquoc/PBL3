using DaNangTourism.Server.DAL;
using MySqlConnector;
using System.Text;
using DaNangTourism.Server.Models.DestinationModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
namespace DaNangTourism.Server.Services
{
    public interface IDestinationService
    {
        IEnumerable<HomeDestination> GetHomeDestinations();
        ListDestinations GetListDestinations(DestinationFilter destinationFilter, int userId = 0);
        DestinationDetail? GetDestinationDetail(int id, int userId = 0);
        void UpdateFavDes(int userId, int favDesId, bool favorite);
        IEnumerable<HomeDestination> GetRandomDestinations(int limit = 3);
        AdminDestinations GetDestinationElements(AdminDestinationFilter adminDestinationFilter);
        int AddDestination(InputDestinationModel destination);
        InputDestinationModel GetDestinationToUpdate(int id);
        InputDestinationModel UpdateDestination(int id, InputDestinationModel destination);
        int DeleteDestination(int id);
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

        /// <summary>
        /// Get list of newest destinations
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HomeDestination> GetHomeDestinations()
        {
            return _destinationRepository.GetNewestDestinations();
        }


        /// <summary>
        /// Get list of destinations
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="destinationFilter"></param>
        /// <returns></returns>
        public ListDestinations GetListDestinations(DestinationFilter destinationFilter, int userId = 0)
        {
            ListDestinations destinations = new ListDestinations();

            // xử lý query để có điều kiện lọc
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT Destinations.destination_id AS DestinationId, name AS Name, address AS Address, images AS Images, IFNULL(AVG(Rating), 0) AS Rating, cost AS Cost, open_time AS OpenTime," +
                "close_time AS CloseTime, tags AS Tags");
            List<MySqlParameter> parameters = new List<MySqlParameter>();

            // xử lý favorite Des: userId = 0 tức là chưa đăng nhập

            sql.Append(", IF(FavDes.user_id = @userId, TRUE, FALSE) as Favorite");
            parameters.Add(new MySqlParameter("@userId", userId));
            sql.Append(" FROM Destinations LEFT JOIN Reviews ON Destinations.destination_id = Reviews.destination_id");
            sql.Append(" LEFT JOIN (SELECT * FROM FavoriteDestinations WHERE user_id = @userId) AS FavDes ON FavDes.destination_id = Destinations.destination_id");

            StringBuilder filter = new StringBuilder();
            // xử lý các bộ lọc dùng where
            if (!string.IsNullOrEmpty(destinationFilter.Search))
            {
                filter.Append(" WHERE name LIKE @search");
                parameters.Add(new MySqlParameter("@search", "%" + destinationFilter.Search + "%"));
            }

            if (!string.IsNullOrEmpty(destinationFilter.Location))
            {
                if (filter.ToString().Contains("WHERE"))
                {
                    filter.Append(" AND address LIKE @location");
                }
                else
                {
                    filter.Append(" WHERE address LIKE @location");
                }
                parameters.Add(new MySqlParameter("@location", "%" + destinationFilter.Location + "%"));
            }

            if (destinationFilter.CostFrom != -1)
            {
                if (filter.ToString().Contains("WHERE"))
                {
                    filter.Append(" AND cost >= @costFrom");
                }
                else
                {
                    filter.Append(" WHERE cost >= @costFrom");
                }
                parameters.Add(new MySqlParameter("@costFrom", destinationFilter.CostFrom));
            }

            if (destinationFilter.CostTo != -1)
            {
                if (filter.ToString().Contains("WHERE"))
                {
                    filter.Append(" AND cost <= @costTo");
                }
                else
                {
                    filter.Append(" WHERE cost <= @costTo");
                }
                parameters.Add(new MySqlParameter("@costTo", destinationFilter.CostTo));
            }

            if (destinationFilter.RatingFrom != -1)
            {
                if (filter.ToString().Contains("WHERE"))
                {
                    filter.Append(" AND AVG(Rating) >= @ratingFrom");
                }
                else
                {
                    filter.Append(" WHERE AVG(Rating) >= @ratingFrom");
                }
                parameters.Add(new MySqlParameter("@ratingFrom", destinationFilter.RatingFrom));
            }

            if (destinationFilter.RatingTo != -1)
            {
                if (filter.ToString().Contains("WHERE"))
                {
                    filter.Append(" AND AVG(Rating) <= @ratingTo");
                }
                else
                {
                    filter.Append(" WHERE AVG(Rating) <= @ratingTo");
                }
                parameters.Add(new MySqlParameter("@ratingTo", destinationFilter.RatingTo));
            }

            if (destinationFilter.IsFavorite != null)
            {
                if (filter.ToString().Contains("WHERE"))
                {
                    filter.Append(" AND ");
                }
                else
                {
                    filter.Append(" WHERE ");
                }
                if (destinationFilter.IsFavorite == false)
                {
                    filter.Append("FavDes.user_id IS NULL");
                }
                else
                {
                    filter.Append("FavDes.user_id IS NOT NULL");
                }
            }

            // thêm group by để sử dụng AVG(Rating)
            filter.Append(" GROUP BY Destinations.destination_id, name, address, images, cost, open_time, close_time, tags");

            // xử lý order by
            string sortBy = (destinationFilter.SortBy == "rating") ? "AVG(Rating)" : "Destinations." + destinationFilter.SortBy;
            filter.Append(" ORDER BY " + sortBy + " " + destinationFilter.SortType);

            sql.Append(filter);

            // Lấy tổng kết quả có được
            StringBuilder countSql = new StringBuilder();
            countSql.Append("SELECT COUNT(*) FROM (" + sql + ") AS subquery");
            destinations.Total = _destinationRepository.GetDestinationCount(countSql.ToString(), parameters.ToArray());

            // Xử lý trang hiển thị và số lượng tương ứng

            // Kiểm tra 
            sql.Append(" LIMIT @limit OFFSET @offset");
            destinations.Limit = destinationFilter.Limit;
            parameters.Add(new MySqlParameter("@limit", destinations.Limit));

            // Kiểm tra page
            destinations.Page = destinationFilter.Page;
            parameters.Add(new MySqlParameter("@offset", (destinations.Page - 1) * destinations.Limit));


            // Lấy dữ liệu từ cơ sở dữ liệu
            destinations.Items = _destinationRepository.GetListDestination(sql.ToString(), parameters).ToList();



            return destinations;
        }

        /// <summary>
        /// Get destination detail by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DestinationDetail? GetDestinationDetail(int id, int userId = 0)
        {
            DestinationDetail? destinationDetail = _destinationRepository.GetDestinationById(id, userId);
            if (destinationDetail == null) return null;
            // xử lý để nhận thông tin cho GeneralReview
            var reviewsCount = _reviewRepository.GetReviewsCountByDesIdGroupedByRating(id);

            //Xử lý nhận tổng số review trước
            foreach (var review in reviewsCount)
            {
                destinationDetail.GeneralReview.TotalReview += review.Value;
            }

            //Xử lý nhận phần trăm các review;
            foreach (var review in reviewsCount)
            {
                destinationDetail.GeneralReview.AddRatingPercent(review.Key, review.Value);
            }

            return destinationDetail;
        }

        /// <summary>
        /// Update favorite destination (add or remove) 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="favDesId"></param>
        /// <param name="favorite"></param>
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

        /// <summary>
        /// Get random destinations
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        public IEnumerable<HomeDestination> GetRandomDestinations(int limit = 3)
        {
            return _destinationRepository.GetRandomDestinations(limit);
        }

        /// <summary>
        /// Get destination elements for admin
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public AdminDestinations GetDestinationElements(AdminDestinationFilter adminDestinationFilter)
        {
            // Khởi tạo đối tượng AdminDestinations
            AdminDestinations adminDestinations = new AdminDestinations();

            // Khởi tạo StringBuilder để xây dựng câu truy vấn SQL
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT d.destination_id AS DestinationId, d.name AS Name, d.address AS Address, IFNULL(AVG(Rating), 0) AS Rating, COUNT(*) AS Review,");
            sql.Append(" (SELECT COUNT(*) FROM FavoriteDestinations f WHERE f.destination_id = d.destination_id) AS Favorite, d.created_at AS Created_At");
            sql.Append(" FROM Destinations d LEFT JOIN Reviews r ON r.destination_id = d.destination_id");

            List<MySqlParameter> parameters = new List<MySqlParameter>();


            // Xử lý lọc dùng WHERE
            if (!string.IsNullOrEmpty(adminDestinationFilter.Search))
            {
                sql.Append(" WHERE d.name LIKE @name");
                parameters.Add(new MySqlParameter("@name", "%" + adminDestinationFilter.Search + "%"));
            }

            // Thêm GROUP BY để sử dụng AVG(Rating), COUNT(*)
            sql.Append(" GROUP BY d.destination_id, d.name, d.address, d.created_at");

            // Xử lý bộ lọc dùng ORDER BY
            string sortBy = (adminDestinationFilter.SortBy == "rating") ? "AVG(Rating)" : "d." + adminDestinationFilter.SortBy;
            sql.Append(" ORDER BY " + sortBy);

            sql.Append(" " + adminDestinationFilter.SortType);

            // Lấy tổng kết quả có được
            StringBuilder countSql = new StringBuilder();
            countSql.Append("SELECT COUNT(*) FROM (" + sql + ") AS subquery");
            adminDestinations.Total = _destinationRepository.GetDestinationCount(countSql.ToString(), parameters.ToArray());

            // Xử lý trang hiển thị và số lượng tương ứng

            // Kiểm tra 
            sql.Append(" LIMIT @limit OFFSET @offset");
            adminDestinations.Limit = adminDestinationFilter.Limit;
            parameters.Add(new MySqlParameter("@limit", adminDestinations.Limit));

            // Kiểm tra page
            adminDestinations.Page = adminDestinationFilter.Page;
            parameters.Add(new MySqlParameter("@offset", (adminDestinations.Page - 1) * adminDestinations.Limit));

            // Lấy dữ liệu từ cơ sở dữ liệu
            adminDestinations.Items = _destinationRepository.GetDestinationElements(sql.ToString(), parameters.ToArray()).ToList();



            return adminDestinations;
        }

        /// <summary>
        /// Add new destination
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        public int AddDestination(InputDestinationModel destination)
        {
            return _destinationRepository.AddDestination(destination);
        }

        /// <summary>
        /// Get destination by id to update destination
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public InputDestinationModel GetDestinationToUpdate(int id)
        {
            return _destinationRepository.GetDestinationToUpdate(id);
        }

        /// <summary>
        /// Update destination
        /// </summary>
        /// <param name="id"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public InputDestinationModel UpdateDestination(int id, InputDestinationModel destination)
        {
            return _destinationRepository.UpdateDestination(id, destination);
        }
        /// <summary>
        /// Delete destination
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteDestination(int id)
        {
            return _destinationRepository.DeleteDestination(id);
        }

    }
}
