using DaNangTourism.Server.Helper;
using DaNangTourism.Server.ModelBindingConverter;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace DaNangTourism.Server.Models.ScheduleModels
{
    public class ScheduleFilter
    {
        [FromQuery(Name = "page")]
        public int Page { get; set; } = 1;

        [FromQuery(Name = "limit")]
        public int Limit { get; set; } = 5;

        [FromQuery(Name = "search")]
        public string Search { get; set; } = "";

        [FromQuery(Name = "status")]
        [JsonConverter(typeof(EnumToStringJsonConverter<ScheduleStatus>))]
        public ScheduleStatus Status { get; set; } = ScheduleStatus.all;

        [FromQuery(Name = "sortBy")]
        public string SortBy { get; set; } = "startDate";

        [FromQuery(Name = "sortType")]
        public string SortType { get; set; } = "desc";

        public void Sanitization()
        {
            if (Page < 1) Page = 1;
            if (Limit < 1) Limit = 1;
            Search = DataSanitization.RemoveSpecialCharacters(Search);
            if (Status != ScheduleStatus.all && Status != ScheduleStatus.planning && Status != ScheduleStatus.ongoing 
                && Status != ScheduleStatus.completed && Status != ScheduleStatus.canceled) Status = ScheduleStatus.all;
            if (SortBy != "title" && SortBy != "startDate" && SortBy != "updatedAt") SortBy = "startDate";
            if (SortType != "asc" && SortType != "desc") SortType = "desc";
        }
    }
}
