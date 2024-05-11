using DaNangTourism.Server.DAL;
using DaNangTourism.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace DaNangTourism.Server.Controllers
{
    [ApiController]
    [Route("destination")]
    public class DestinationController : Controller
    {
        [HttpGet("get/all")]
        public IActionResult GetAllDestinations()
        {
            DestinationDAO destinationDAO = new DestinationDAO();
            Dictionary<int, Destination> destinations = destinationDAO.GetAllDestinations();
            if (destinations.Count == 0 )
            {
                return NotFound();
            }
            else return Ok(destinations);
        }
        [HttpGet("get/{id}")]
        public IActionResult GetDestinationById(int id)
        {
            DestinationDAO destinationDAO = new DestinationDAO();
            Destination destination = destinationDAO.GetDestinationsById(id);
            if (destination == null)
            {
                return NotFound();
            }
            else return Ok(destination);
        }
        //[HttpPost("add")]
        //public IActionResult AddDestination([FromBody] Destination destination)
        //{
        //    DestinationDAO destinationDAO = new DestinationDAO();
        //    destinationDAO.Add()
        //}
        [HttpPut("update")]
        public IActionResult UpdateDestination([FromBody] Destination destination)
        {
            DestinationDAO destinationDAO = new DestinationDAO();
            bool check = (destinationDAO.UpdateDestination(destination) > 1) ? true: false;
            if (check)
            {
                return NoContent();
            }
            else return BadRequest();
        }
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteDestination(int id)
        {
            DestinationDAO destinationDAO = new DestinationDAO();
            bool check = (destinationDAO.DeleteDestination(id) > 1) ? true : false;
            if (check)
            {
                return NoContent();
            }
            else return BadRequest();
        }
        [HttpGet("Sort/Rating")]
        public IActionResult SortDestinationByRating()
        {
            DestinationDAO destinationDAO = new DestinationDAO();
            List<Destination> destinations = destinationDAO.GetDescendingDestination();
            if (destinations.Count == 0)
            {
                return NotFound();
            }
            else return Ok(destinations);
        }
    }
}
