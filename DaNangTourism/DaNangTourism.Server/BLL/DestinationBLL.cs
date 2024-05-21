using DaNangTourism.Server.DAL;
using DaNangTourism.Server.Models;
using DaNangTourism.Server.Models.DestinationModels;
using Microsoft.AspNetCore.Components.Web;
namespace DaNangTourism.Server.BLL
{
    public class DestinationBLL
    {
        public List<HomeDestination> GetHomeDestinations()
        {
            DestinationDAO destinationDAO = DestinationDAO.Instance;
            return destinationDAO.GetNewestDestinations();
        }
        public List<ListDestination> GetListDestinations(int userId, IQueryCollection query)
        {
            DestinationDAO destinationDAO = DestinationDAO.Instance;
            // xử lý query để có điều kiện lọc
            string filter = "";

            // xử lý các bộ lọc dùng where
            if ( query.ContainsKey("search") || query.ContainsKey("location") 
                || query.ContainsKey("costFrom") || query.ContainsKey("costTo")
                || query.ContainsKey("ratingFrom") || query.ContainsKey("ratingTo")
                || query.ContainsKey("isFavorite") )
            {
                if (query.ContainsKey("isFavorite")) filter += " left join FavoriteDestination";
                filter += " where";
                int count = 0;
                if (query.ContainsKey("isFavorite"))
                {
                    count++;
                    if (count > 1) filter += " and";
                    if (query["isFavorite"].Equals("true"))
                    {
                        filter += " UserId = " + userId;
                    }    
                    else if (query["isFavorite"].Equals("false"))
                    {
                        filter += " UserId != " + userId;
                    }    
                }    
                if (query.ContainsKey("search"))
                {
                    count++;
                    if (count > 1) filter += " and";
                    filter += " Name = '" + query["search"] + "'";
                } 
                if (query.ContainsKey("location"))
                {
                    count++;
                    if (count > 1) filter += " and";
                    filter += " Location like '%" + query["location"] + "%'";
                }
                if (query.ContainsKey("costFrom"))
                {
                    count++;
                    if (count > 1) filter += " and";
                    filter += " Cost >= " + query["costFrom"];
                }
                if (query.ContainsKey("costTo"))
                {
                    count++;
                    if (count > 1) filter += " and";
                    filter += " Cost <= " + query["costTo"];
                }
                if (query.ContainsKey("ratingFrom"))
                {
                    count++;
                    if (count > 1) filter += " and";
                    filter += " Rating >= " + query["ratingFrom"];
                }
                if (query.ContainsKey("ratingTo"))
                {
                    count++;
                    if (count > 1) filter += " and";
                    filter += " Rating <= " + query["ratingTo"];
                }
            }

            // xử lý bộ lọc dùng order by
            filter += " order by ";
            if (query.ContainsKey("sortBy"))
            {
                filter += " " + query["sortBy"];
                // Bảo sửa price thành cost
            }
            else filter += "created_at";
            if (query.ContainsKey("sortType"))
            {
                filter += " " + query["sortType"];
            }

            //xử lý trang hiển thị và số lượng tương ứng
            filter += " limit";
            int limit = 12;
            if (query.ContainsKey("limit"))
            {
                limit = Int32.Parse(query["limit"]);
                filter += " " + limit;
            }
            if (query.ContainsKey("page"))
            {
                int page = Int32.Parse(query["page"]);
                filter += "offset " + (page - 1) * 12;
            }
            
            

            List<ListDestination> destinations = destinationDAO.GetListDestination(filter);
            // Xử lý để xem các destination có phải là yêu thích không
            if (query.ContainsKey("isFavorite"))
            {
                if (query["isFavortie"].Equals("true"))
                {
                    foreach (ListDestination destination in destinations)
                    {
                        destination.Favourite = true;
                    }
                }
            }
            else
            {
                foreach (int favDesId in FavoriteDestinationDAO.Instance.GetFavDesIds(userId))
                {
                    ListDestination? destination = destinations.Find(destination => destination.Id == favDesId);
                    if (destination != null)
                    {
                        destination.Favourite = true;
                    }
                }
            }
              
            return destinations;
        }

        public DestinationDetail? GetDestinationDetail(int id)
        {
            DestinationDetail? destinationDetail = DestinationDAO.Instance.GetDestinationById(id);
            if (destinationDetail == null) return null;
            // xử lý để nhận thông tin cho GeneralReview
            destinationDetail.GeneralReview.TotalReview = ReviewDAO.Instance.GetReviewsCountByDesId(id);
            for (int i = 5; i >= 1; i--)
            {
                int countOfRating = ReviewDAO.Instance.GetReviewsCountByDesId(id, i);
                destinationDetail.GeneralReview.AddRatingPercent(i, countOfRating);
            }    
            return destinationDetail;
        }

        public List<DestinationReview> GetDestinationReviews(int destinationId)
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
            return ReviewDAO.Instance.AddReview(userId, destinationId, review);
        }

        public void UpdateFavDes(int userId, int favDesId, bool favorite)
        {
            if (favorite)
            {
                FavoriteDestinationDAO.Instance.AddFavDes(userId, favDesId);
            }
            else
            {
                FavoriteDestinationDAO.Instance.DeleteFavDes(userId, favDesId);
            }
        }
        public List<HomeDestination> GetRandomDestinations(IQueryCollection query)
        {
            if (query.ContainsKey("limit"))
            {
                int limit = Int32.Parse(query["limit"]);
                DestinationDAO.Instance.GetRandomDestinations(limit);
            }    
            return DestinationDAO.Instance.GetRandomDestinations();
        }

        public List<DestinationElement> GetDestinationElements(IQueryCollection query)
        {
            string sql = "Select d.DestinationId, d.Name, d.Address, d.Rating, d.Created_At, " +
                "(Select count(*) from Reviews r where r.DestinationId = d.DestinationId) as Review, " +
                "(Select count(*) from FavoriteDestinations where f.DestinationId = d.DestinationId) as Favourite, " +
                "d.Created_At from Destination d";
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
            int limit = 12;
            if (query.ContainsKey("limit"))
            {
                limit = Int32.Parse(query["limit"]);
                filter += " " + limit;
            }
            if (query.ContainsKey("page"))
            {
                int page = Int32.Parse(query["page"]);
                filter += "offset " + (page - 1) * limit;
            }

            List<DestinationElement> destinations = DestinationDAO.Instance.GetDestinationElements(sql + filter);
            return destinations;
        }

        //hụt phần user
        public List<DestinationReview> GetReviewsByDesId(int destinationId, IQueryCollection query)
        {
            string filter = "";
            if (query.ContainsKey("sortBy"))
            {

            }
            return null;
        }
        public int AddDestination (DestinationModel destination)
        {
            return DestinationDAO.Instance.AddDestination(destination);
        }
    }
}
