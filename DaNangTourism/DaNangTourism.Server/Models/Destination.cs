using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Hosting;
using MySqlConnector;

namespace DaNangTourism.Server.Models
{
    public class Destination
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? LocalName { get; set; }
        public string? Address { get; set; }
        public string[] Images { get; set; }
        public double Cost { get; set; }
        public TimeOnly OpenTime { get; set; }
        public TimeOnly CloseTime { get; set; }
        public string[] Tags { get; set; }
        public string Introduction { get; set; }
        public string GoogleMapURL { get; set; }
        public float Rating { get; set; }

        public Destination()
        {
            Images = new string[0];
            Tags = new string[0];
        }
        public Destination(MySqlDataReader reader)
        {
            Id = reader.GetInt32("Id");
            Name = reader.GetString("Name");
            LocalName = reader.GetString("LocalName");
            Address = reader.GetString("Address");
            Images = reader.GetString("Images").Split(';');
            Cost = reader.GetDouble("Cost");
            OpenTime = reader.GetTimeOnly("OpenTime");
            CloseTime = reader.GetTimeOnly("CloseTime");
            Tags = reader.GetString("Tags").Split(';');
            Introduction = reader.GetString("Introduction");
            GoogleMapURL = reader.GetString("GoogleMapURL");
            Rating = reader.GetFloat("rating");
        }
    }
    public class HomeDestination
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Image { get; set; }
        public double Rating { get; set; }
        public HomeDestination(MySqlDataReader reader)
        {
            Id = reader.GetInt32("Id");
            Name = reader.GetString("Name");
            Address = reader.GetString("Address");
            Image = reader.GetString("Images").Split(';').FirstOrDefault();
            Rating = reader.GetFloat("Rating");
        }
    }
    public class ListDestination
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Image { get; set; }
        public double Cost { get; set; }
        public double Rating { get; set; }
        public TimeOnly OpenTime { get; set; }
        public TimeOnly CloseTime { get; set; }
        public string[] Tags { get; set; }
        public bool Favourite { get; set; }

        public ListDestination(MySqlDataReader reader)
        {
            Id = reader.GetInt32("Id");
            Name = reader.GetString("Name");
            Address = reader.GetString("Address");
            Image = reader.GetString("Images").Split(';').FirstOrDefault();
            Rating = reader.GetFloat("Rating");
            Cost = reader.GetDouble("Cost");
            OpenTime = reader.GetTimeOnly("OpenTime");
            CloseTime = reader.GetTimeOnly("CloseTime");
            Tags = reader.GetString("Tags").Split(';');
            // giá trị mặc định là không phải yêu thích
            Favourite = false;
        }
    }
}
