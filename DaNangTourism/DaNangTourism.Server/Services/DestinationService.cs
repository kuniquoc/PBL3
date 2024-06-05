using DaNangTourism.Server.DAL;
using MySqlConnector;
using System.Text;
using DaNangTourism.Server.Models.DestinationModels;
using Microsoft.AspNetCore.Mvc;
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
      sql.Append("SELECT Destinations.DestinationId AS DestinationId, Name, Address, Images, Rating, Cost, OpenTime, CloseTime, Tags");
      List<MySqlParameter> parameters = new List<MySqlParameter>();

      // xử lý favorite Des
      if (userId > 0)
      {
        sql.Append(", IF(UserId = @userId, TRUE, FALSE) as Favorite");
        parameters.Add(new MySqlParameter("@userId", userId));
      }
      else
      {
        sql.Append(", FALSE as Favorite");
      }
      sql.Append(" FROM Destinations LEFT JOIN FavoriteDestinations ON FavoriteDestinations.DestinationId = Destinations.DestinationId");


      // xử lý các bộ lọc dùng where
      if (!string.IsNullOrEmpty(destinationFilter.Search))
      {
        sql.Append(" WHERE Name LIKE @search");
        parameters.Add(new MySqlParameter("@search", "%" + destinationFilter.Search + "%"));
      }

      if (!string.IsNullOrEmpty(destinationFilter.Location))
      {
        if (sql.ToString().Contains("WHERE"))
        {
          sql.Append(" AND Address LIKE @location");
        }
        else
        {
          sql.Append(" WHERE Address LIKE @location");
        }
        parameters.Add(new MySqlParameter("@location", "%" + destinationFilter.Location + "%"));
      }

      if (destinationFilter.CostFrom != -1)
      {
        if (sql.ToString().Contains("WHERE"))
        {
          sql.Append(" AND Cost >= @costFrom");
        }
        else
        {
          sql.Append(" WHERE Cost >= @costFrom");
        }
        parameters.Add(new MySqlParameter("@costFrom", destinationFilter.CostFrom));
      }

      if (destinationFilter.CostTo != -1)
      {
        if (sql.ToString().Contains("WHERE"))
        {
          sql.Append(" AND Cost <= @costTo");
        }
        else
        {
          sql.Append(" WHERE Cost <= @costTo");
        }
        parameters.Add(new MySqlParameter("@costTo", destinationFilter.CostTo));
      }

      if (destinationFilter.RatingFrom != -1)
      {
        if (sql.ToString().Contains("WHERE"))
        {
          sql.Append(" AND Rating >= @ratingFrom");
        }
        else
        {
          sql.Append(" WHERE Rating >= @ratingFrom");
        }
        parameters.Add(new MySqlParameter("@ratingFrom", destinationFilter.RatingFrom));
      }

      if (destinationFilter.RatingTo != -1)
      {
        if (sql.ToString().Contains("WHERE"))
        {
          sql.Append(" AND Rating <= @ratingTo");
        }
        else
        {
          sql.Append(" WHERE Rating <= @ratingTo");
        }
        parameters.Add(new MySqlParameter("@ratingTo", destinationFilter.RatingTo));
      }

      if (destinationFilter.IsFavorite != null)
      {
        if (sql.ToString().Contains("WHERE"))
        {
          sql.Append(" AND UserId = @userId");
        }
        else
        {
          sql.Append(" WHERE UserId = @userId");
        }
        parameters.Add(new MySqlParameter("@userId", userId));
      }

      // xử lý order by
      sql.Append(" ORDER BY " + destinationFilter.SortBy + " " + destinationFilter.SortType);

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
      sql.Append("SELECT d.DestinationId AS DestinationId, d.Name AS Name, d.Address AS Address, d.Rating AS Rating, ");
      sql.Append("(SELECT COUNT(*) FROM Reviews r WHERE r.DestinationId = d.DestinationId) AS CountOfReview, ");
      sql.Append("(SELECT COUNT(*) FROM FavoriteDestinations f WHERE f.DestinationId = d.DestinationId) AS CountOfFavorite, ");
      sql.Append("d.Created_At AS Created_At FROM Destinations d");

      List<MySqlParameter> parameters = new List<MySqlParameter>();


      // Xử lý lọc dùng WHERE
      if (!string.IsNullOrEmpty(adminDestinationFilter.Search))
      {
        sql.Append(" WHERE Name LIKE @name");
        parameters.Add(new MySqlParameter("@name", "%" + adminDestinationFilter.Search + "%"));
      }

      // Xử lý bộ lọc dùng ORDER BY
      sql.Append(" ORDER BY ");
      if (!string.IsNullOrEmpty(adminDestinationFilter.SortBy))
      {
        sql.Append(adminDestinationFilter.SortBy);
      }
      else
      {
        sql.Append("created_at");
      }

      if (!string.IsNullOrEmpty(adminDestinationFilter.SortType))
      {
        sql.Append(" ").Append(adminDestinationFilter.SortType);
      }

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
