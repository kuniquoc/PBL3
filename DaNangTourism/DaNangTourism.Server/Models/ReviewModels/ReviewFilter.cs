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
        if (Page < 1)
        {
            Page = 1;
        }
        if (Limit < 1)
        {
            Limit = 3;
        }
        if (SortBy != "created_at" && SortBy != "rating")
        {
            SortBy = "created_at";
        }
        if (SortType != "asc" && SortType != "desc")
        {
            SortType = "desc";
        }
    }
}
