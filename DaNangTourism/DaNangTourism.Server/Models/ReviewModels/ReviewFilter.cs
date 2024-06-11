using DaNangTourism.Server.Helper;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace DaNangTourism.Server.Models.ReviewModels;

public class ReviewFilter
{
    [FromQuery(Name = "page")]
    public int Page { get; set; } = 1;
    [FromQuery(Name = "limit")]
    public int Limit { get; set; } = 3;
    [FromQuery(Name = "sortBy")]
    public string SortBy { get; set; } = "created_at";
    [FromQuery(Name = "sortType")]
    public string SortType { get; set; } = "desc";

    public void Sanitization()
    {
        SortBy = DataSanitization.RemoveSpecialCharacters(SortBy);
        SortType = DataSanitization.RemoveSpecialCharacters(SortType);
        if (Page < 1)
        {
            Page = 1;
        }
        if (Limit < 1)
        {
            Limit = 3;
        }
        if (!SortBy.Equals("created_at") && !SortBy.Equals("rating"))
        {
            SortBy = "created_at";
        }
        if (!SortType.Equals("asc") && !SortType.Equals("desc"))
        {
            SortType = "desc";
        }
        
    }
}
