﻿using Microsoft.Extensions.Hosting;
using MySqlConnector;
using static System.Net.Mime.MediaTypeNames;
using System.Net;
using System.Xml.Linq;
using System.Text.Json.Serialization;

namespace DaNangTourism.Server.Models
{
    public class DestinationDetail
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("information")]
        public DesInfo Information { get; set; }
        [JsonPropertyName("introduction")]
        public string Introduction { get; set; }
        [JsonPropertyName("googleMapUrl")]
        public string GoogleMapUrl { get; set; }
        [JsonPropertyName("generalReview")]
        public DesGeneralReview GeneralReview { get; set; }
        public DestinationDetail(MySqlDataReader reader)
        {
            Id = reader.GetInt32("DestinationId");
            string Name = reader.GetString("Name");
            string LocalName = reader.GetString("LocalName");
            string Address = reader.GetString("Address");
            string[] Images = reader.GetString("Images").Split(';');
            double Cost = reader.GetDouble("Cost");
            TimeOnly OpenTime = reader.GetTimeOnly("OpenTime");
            TimeOnly CloseTime = reader.GetTimeOnly("CloseTime");
            string[] Tags = reader.GetString("Tags").Split(';');
            Information = new DesInfo(Name, LocalName, Address, Images, Cost, OpenTime, CloseTime, Tags);
            Introduction = reader.GetString("Introduction");
            GoogleMapUrl = reader.GetString("GoogleMapUrl");
            GeneralReview = new DesGeneralReview();
            GeneralReview.Rating = reader.GetFloat("Rating");
        }
    }
    public class DesInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("localName")]
        public string LocalName { get; set; }
        [JsonPropertyName("address")]
        public string Address { get; set; }
        [JsonPropertyName("images")]
        public string[] Images { get; set; }
        [JsonPropertyName("cost")]
        public double Cost { get; set; }
        [JsonPropertyName("openTime")]
        public TimeOnly OpenTime { get; set; }
        [JsonPropertyName("closeTime")]
        public TimeOnly CloseTime { get; set; }
        [JsonPropertyName("tags")]
        public string[] Tags { get; set; }
        public DesInfo(string name, string localName, string address, string[] images, double cost, TimeOnly openTime, TimeOnly closeTime, string[] tags)
        {
            Name = name;
            LocalName = localName;
            Address = address;
            Images = images;
            Cost = cost;
            OpenTime = openTime;
            CloseTime = closeTime;
            Tags = tags;
        }
    }
    public class DesGeneralReview
    {
        [JsonPropertyName("rating")]
        public float Rating { get; set; }
        [JsonPropertyName("totalReview")]
        public int TotalReview { get; set; }
        [JsonPropertyName("detail")]
        public Dictionary<int, float> Detail { get; set; }
        public DesGeneralReview()
        {
            Detail = new Dictionary<int, float>();
        }
        /// <summary>
        /// Add percent of rating into Detail
        /// </summary>
        /// <param name="rating"></param>
        /// <param name="countOfRating"></param>
        public void AddRatingPercent (int rating, int countOfRating)
        {
            float percent = (float)countOfRating / TotalReview;
            Detail.Add(rating, percent);
        }
    }
}