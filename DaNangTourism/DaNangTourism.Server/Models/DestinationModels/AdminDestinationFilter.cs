using DaNangTourism.Server.Helper;

namespace DaNangTourism.Server.Models.DestinationModels
{
  public class AdminDestinationFilter
  {
    public int Page { get; set; } = 1;
    public int Limit { get; set; } = 15;
    public string Search { get; set; } = "";
    public string SortBy { get; set; } = "created_at";
    public string SortType { get; set; } = "asc";

    // Function to sanitize the input data
    public void Sanitization()
    {
      if (Page < 1) Page = 1;
      if (Limit < 1) Limit = 12;
      if (SortBy != "created_at" && SortBy != "name" && SortBy != "rating" && SortBy != "review" && SortBy != "favorite") SortBy = "created_at";
      else if (SortBy == "favorite") SortBy = "CountOfFavorite";
      else if (SortBy == "review") SortBy = "CountOfReview";

      if (SortType != "asc" && SortType != "desc") SortType = "asc";
      Search = DataSanitization.RemoveSpecialCharacters(Search);
    }
  }
}
