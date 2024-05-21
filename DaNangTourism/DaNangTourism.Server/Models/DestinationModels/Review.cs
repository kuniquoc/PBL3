﻿using MySqlConnector;
using System.Text.Json.Serialization;

namespace DaNangTourism.Server.Models
{
    public class Review
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonIgnore] 
        public int UserId { get; set; }
        [JsonPropertyName("rating")]
        public int Rating { get; set; }
        [JsonPropertyName("comment")]
        public string Comment { get; set; }
        public DateTime Created_At { get; set; }
        public Review()
        {
            Comment = "";
        }
        public Review (MySqlDataReader reader)
        {
            Id = reader.GetInt32("ReviewId");
            UserId = reader.GetInt32("UserId");
            Rating = reader.GetInt32("Rating");
            Comment = reader.GetString("Comment");
        }
    }
}