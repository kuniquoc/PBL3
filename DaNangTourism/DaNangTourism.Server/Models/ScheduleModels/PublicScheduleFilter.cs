using DaNangTourism.Server.Helper;
using Microsoft.AspNetCore.Mvc;

namespace DaNangTourism.Server.Models.ScheduleModels
{
    public class PublicScheduleFilter
    {
        [FromQuery(Name = "page")]
        public int Page { get; set; } = 1;

        [FromQuery(Name = "limit")]
        public int Limit { get; set; } = 5;

        [FromQuery(Name = "search")]
        public string Search { get; set; } = "";

        [FromQuery(Name = "sortBy")]
        public string SortBy { get; set; } = "title";

        [FromQuery(Name = "sortType")]
        public string SortType { get; set; } = "desc";

        public void Sanitization()
        {
            if (Page < 1) Page = 1;
            if (Limit < 1) Limit = 1;
            Search = DataSanitization.RemoveSpecialCharacters(Search);
            if (SortBy != "title" && SortBy != "startDate" && SortBy != "updatedAt") SortBy = "startDate";
            if (SortType != "asc" && SortType != "desc") SortType = "desc";
        }
    }
}
