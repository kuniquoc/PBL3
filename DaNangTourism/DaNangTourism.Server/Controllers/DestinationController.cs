using DaNangTourism.Server.BLL;
using DaNangTourism.Server.Models;
using DaNangTourism.Server.Models.DestinationModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Primitives;

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
                return StatusCode(500, "Server error");
            }
        }
        [HttpGet("list")]
        public IActionResult GetListDestinations(IQueryCollection query)
        {
            int userId = 0;
            if (HttpContext.Request.Cookies.ContainsKey("token"))
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
                return StatusCode(500, "Server error");
            }
        }
        [HttpGet("detail/{id}")]
        public IActionResult GetDestinationById([FromRoute] int id)
        {
            try
            {
                DestinationBLL db = new DestinationBLL();
                var destination = db.GetDestinationDetail(id);
                if (destination == null)
                {
                    return NotFound();
                }
                else return Ok(destination);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, "Server error");
            }

        }
        [HttpGet("review/{id}")]
        public IActionResult GetReviewsByDestinationId([FromRoute] int id)
        {
            // chờ phần user
            return Ok();
        }
        [HttpPost("review")]
        public IActionResult AddReview([FromBody] Review review)
        {
            review.Created_At = DateTime.Now;
            int userId = 0;
            if (HttpContext.Request.Cookies.ContainsKey("token"))
            {
                //xác thực token và nhận id
                userId = 0;
                return Unauthorized();
            }
            else
            {
                int destinationId = 0;
                if (!Request.Form.ContainsKey("destinationId"))
                {
                    return BadRequest();
                }
                else
                {
                    destinationId = Int32.Parse(Request.Form["destinationId"].ToString());
                    try
                    {
                        DestinationBLL db = new DestinationBLL();
                        int reviewId = db.AddReview(userId, destinationId, review);
                        return CreatedAtAction("Review created", new {id = reviewId}, review);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        return StatusCode(500, "Server error");
                    }
                }
            }
        }
        [HttpPut("favorite/{destinationId}")]
        public IActionResult UpdateFavDes([FromRoute] int destinationId, [FromBody] bool favorite)
        {
            int userId = 0;
            if (HttpContext.Request.Cookies.ContainsKey("token"))
            {
                //xác thực token và nhận id
                userId = 0;
                return Unauthorized();
            }
            else
            {
                try
                {
                    DestinationBLL db = new DestinationBLL();
                    db.UpdateFavDes(userId, destinationId, favorite);
                    return Ok("Favorite updated");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return StatusCode(500, "Server error");
                }
            }
        }
        [HttpGet("random")]
        public IActionResult GetRandomDestinations(IQueryCollection query)
        {
            try
            {
                DestinationBLL db = new DestinationBLL();
                List<HomeDestination> destinations = db.GetRandomDestinations(query);
                if (destinations.Count == 0)
                {
                    return NotFound();
                }
                else return Ok(destinations);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, "Server error");
            }
        }
        [HttpGet("managelist")]
        public IActionResult GetAdminList(IQueryCollection query, [FromBody] AdminDestinations adminDestinations)
        {
            // xác thực admin


            //
            try
            {
                DestinationBLL db = new DestinationBLL();
                adminDestinations.Items = db.GetDestinationElements(query);
                if (adminDestinations.Items.Count == 0)
                {
                    return NotFound();
                }
                else return Ok(adminDestinations);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, "Server error");
            }
        }
        [HttpPost("create")]
        public IActionResult CreateNewDestination([FromBody] DestinationModel destination)
        {
            try
            {
                DestinationBLL db = new DestinationBLL();
                int destinationId = db.AddDestination(destination);
                return CreatedAtAction("Destination created", new { id = destinationId }, destination);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, "Server error");
            }
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