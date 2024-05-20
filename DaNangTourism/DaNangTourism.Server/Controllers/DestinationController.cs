using DaNangTourism.Server.BLL;
using DaNangTourism.Server.DAL;
using DaNangTourism.Server.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace DaNangTourism.Server.Controllers
{
    [ApiController]
    [Route("destination")]
    public class DestinationController : Controller
    {
        [HttpGet("home")]
        public IActionResult GetHomeDestinations()
        {
            try
            {
                DestinationBLL db = new DestinationBLL();
                var destinations = db.GetHomeDestinations();
                if (destinations.Count == 0)
                {
                    return NotFound();
                }
                else return Ok(destinations);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, "Lỗi server");
            }
        }
        [HttpGet("list")]
        public IActionResult GetListDestinations(IQueryCollection query)
        {
            string? token;
            int userId = 0;
            if (HttpContext.Request.Cookies.TryGetValue("token", out token))
            {
                //xác thực token và nhận id

            }
            try
            {
                DestinationBLL db = new DestinationBLL();
                var destinations = db.GetListDestinations(userId, query);
                if (destinations.Count == 0)
                {
                    return NotFound();
                }
                else return Ok(destinations);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, "Lỗi server");
            }
        }
        [HttpGet("detail/{id}")]
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
        [HttpGet("review/{id}")]
        public IActionResult GetReviewsByDestinationId([FromRoute] int id)
        {
            return Ok();
        }
        [HttpPost("review")]
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
        [HttpPut("favorite/{destinationId}")]
        public IActionResult UpdateDestination([FromRoute] int destinationId, [FromBody] bool favorite)
        {
            return Ok();
        }
        [HttpGet("random")]
        public IActionResult GetRandomDestinations(IQueryCollection query)
        {
            return Ok();
        }
        [HttpGet("managelist")]
        public IActionResult GetAdminList(IQueryCollection query)
        {
            return Ok();
        }
        [HttpPost("create")]
        public IActionResult CreateNewDestination(IQueryCollection query)
        {
            return Ok();
        }
        [HttpPut("update/{id}")]
        public IActionResult UpdateDestination([FromRoute] int id)
        {
            return Ok();
        }
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteDestination([FromRoute] int id)
        {
            return Ok();
        }
    }
}