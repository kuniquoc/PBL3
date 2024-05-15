using DaNangTourism.Server.DAL;
using DaNangTourism.Server.Models;
using Microsoft.AspNetCore.Http.HttpResults;
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
            DestinationDAO destinationDAO = DestinationDAO.Instance;
            Dictionary<int, Destination> destinations = destinationDAO.GetAllDestinations();
            List<Destination> destinations1 = destinations.Values.ToList();
            if (destinations.Count == 0 )
            {
                return NotFound();
            }
            else return Ok(destinations1);
        }
        [HttpGet("get/{id}")]
        public IActionResult GetDestinationById([FromRoute] int id)
        {
            DestinationDAO destinationDAO = DestinationDAO.Instance;
            Destination? destination = destinationDAO.GetDestinationById(id);
            if (destination == null)
            {
                return NotFound();
            }
            else return Ok(destination);
        }
        [HttpPost("add")]
        public IActionResult AddDestination([FromBody] Destination destination)
        {
            DestinationDAO destinationDAO = DestinationDAO.Instance;
            bool check = destinationDAO.AddDestination(destination) > 0;
            if (check)
            {
                return Ok();
            }
            else return BadRequest();
        }
        [HttpPut("update")]
        public IActionResult UpdateDestination([FromBody] Destination destination)
        {
            DestinationDAO destinationDAO = DestinationDAO.Instance;
            bool check = destinationDAO.UpdateDestination(destination) > 0 ;
            if (check)
            {
                return Ok();
            }
            else return BadRequest();
        }
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteDestination([FromRoute] int id)
        {
            DestinationDAO destinationDAO = DestinationDAO.Instance; 
            bool check = destinationDAO.DeleteDestination(id) > 0;
            if (check)
            {
                return Ok();
            }
            else return BadRequest();
        }
        [HttpGet("Sort/Rating")]
        public IActionResult SortDestinationByRating()
        {
            DestinationDAO destinationDAO = DestinationDAO.Instance;
            List<Destination> destinations = destinationDAO.GetDescendingDestination();  
            if (destinations.Count == 0)
            {
                return NotFound();
            }
            else return Ok(destinations);
        }
    }
}
