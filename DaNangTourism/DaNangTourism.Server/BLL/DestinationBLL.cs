using DaNangTourism.Server.DAL;
using DaNangTourism.Server.Models;
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
            DestinationDAO destinationDAO= DestinationDAO.Instance;
            // xử lý query để có điều kiện lọc
            string filter = "";

            List<ListDestination> destinations = destinationDAO.GetListDestination(filter);
            // Xử lý để xem các destination có phải là yêu thích không
            foreach (int favDesId in FavouriteDestinationDAO.Instance.GetFavDesIds(userId))
            {
                ListDestination? destination = destinations.Find(destination => destination.Id == favDesId);
                if (destination != null)
                {
                    destination.Favourite = true;
                }         
            }    
            return destinations;
        }
    }
}
