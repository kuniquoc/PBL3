using DaNangTourism.Server.Helper;

namespace DaNangTourism.Server.Models.DestinationModels
{
  public class FavoriteModel
  {
    public int DestinationId { get; set; }
    public bool IsFavorite { get; set; } = false;
  }
}
