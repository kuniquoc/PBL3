using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace DaNangTourism.Server.Models
{
    public class DestinationFilter
    {
        [FromQuery(Name = "page")]
        public int Page { get; set; } = 1;

        [FromQuery(Name = "limit")]
        public int Limit { get; set; } = 12;

        [FromQuery(Name = "search")]
        public string Search { get; set; } = "";

        [FromQuery(Name = "location")]
        public string Location { get; set; } = "";

        [FromQuery(Name = "costFrom")]
        public double CostFrom { get; set; } = -1;

        [FromQuery(Name = "costTo")]
        public double CostTo { get; set; } = -1;

        [FromQuery(Name = "ratingFrom")]
        public float RatingFrom { get; set; } = -1;

        [FromQuery(Name = "ratingTo")]
        public float RatingTo { get; set; } = -1;

        [FromQuery(Name = "isFavorite")]
        public bool? IsFavorite { get; set; } = null;

        [FromQuery(Name = "sortBy")]
        public string SortBy { get; set; } = "created_at";

        [FromQuery(Name = "sortType")]
        public string SortType { get; set; } = "asc";
    }
}
