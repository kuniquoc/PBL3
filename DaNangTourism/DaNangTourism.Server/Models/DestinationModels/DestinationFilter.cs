using DaNangTourism.Server.Validation;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace DaNangTourism.Server.Models.DestinationModels
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

        // Function to sanitize the input data
        public void Sanitization()
        {
            if (Page < 1) Page = 1;
            if (Limit < 1) Limit = 12;
            if (CostFrom < -1) CostFrom = -1;
            if (CostTo < -1) CostTo = -1;
            if (RatingFrom < -1) RatingFrom = -1;
            if (RatingTo < -1) RatingTo = -1;
            if (SortBy != "created_at" && SortBy != "name" && SortBy != "cost" && SortBy != "rating") SortBy = "created_at";
            if (SortType != "asc" && SortType != "desc") SortType = "asc";
            Search = DataSanitization.RemoveSpecialCharacters(Search);
            Location = DataSanitization.RemoveSpecialCharacters(Location);
        }
    }
}
