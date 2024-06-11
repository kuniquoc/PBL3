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
            SortBy = DataSanitization.RemoveSpecialCharacters(SortBy);
            SortType = DataSanitization.RemoveSpecialCharacters(SortType);
            if (Page < 1) Page = 1;
            if (Limit < 1) Limit = 1;
            Search = DataSanitization.RemoveSpecialCharacters(Search);
            if (!SortBy.Equals("title") && !SortBy.Equals("startDate") && !SortBy.Equals("updatedAt")) SortBy = "startDate";
            if (SortBy.Equals("updatedAt")) SortBy = "updated_at";
            if (!SortType.Equals("asc") && !SortType.Equals("desc")) SortType = "desc";
        }
    }
}
