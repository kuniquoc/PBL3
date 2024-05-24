using DaNangTourism.Server.Models;
using DaNangTourism.Server.Service;
using DaNangTourism.Server.Helper;
using Microsoft.AspNetCore.Mvc;

namespace DaNangTourism.Server.Controllers
{
    [ApiController]
    [Route("destination")]
    public class DestinationController : Controller
    {
        private readonly IDestinationService _destinationService;
        private readonly IAuthenticationHelper _authenticationHelper;
        public DestinationController (IDestinationService destinationService, IAuthenticationHelper authenticationHelper)
        {
            _destinationService = destinationService;
            _authenticationHelper = authenticationHelper;
        }
        [HttpGet("home")]
        public IActionResult GetHomeDestinations()
        {
            try
            {
                var destinations = _destinationService.GetHomeDestinations();
                if (destinations.Count() == 0)
                {
                    return NotFound();
                }
                else return Ok(destinations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        [HttpGet("list")]
        public IActionResult GetListDestinations([FromQuery] DestinationFilter destinationFilter)
        {
            try
            {
                int userId = _authenticationHelper.GetUserIdFromToken();
                var destinations = _destinationService.GetListDestinations(userId, destinationFilter);
                if (destinations.Count() == 0)
                {
                    return NotFound();
                }
                else return Ok(destinations);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        [HttpGet("detail/{id}")]
        public IActionResult GetDestinationById([FromRoute] int id)
        {
            try
            {
                var destination = _destinationService.GetDestinationDetail(id);
                if (destination == null)
                {
                    return NotFound();
                }
                else return Ok(destination);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
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
                        int reviewId = _destinationService.AddReview(userId, destinationId, review);
                        return CreatedAtAction("Review created", new { id = reviewId }, review);
                    }
                    catch (Exception e)
                    {
                        return StatusCode(500, e.Message);
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
                    _destinationService.UpdateFavDes(userId, destinationId, favorite);
                    return Ok("Favorite updated");
                }
                catch (Exception e)
                {
                    return StatusCode(500, e.Message);
                }
            }
        }
        [HttpGet("random")]
        public IActionResult GetRandomDestinations([FromQuery] IQueryCollection query)
        {
            try
            {
                var destinations = _destinationService.GetRandomDestinations(query);
                if (destinations.Count() == 0)
                {
                    return NotFound();
                }
                else return Ok(destinations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        [HttpGet("managelist")]
        public IActionResult GetAdminList([FromQuery] IQueryCollection query)
        {
            // xác thực admin


            //
            try
            {
                AdminDestinations adminDestinations = _destinationService.GetDestinationElements(query);
                if (adminDestinations.Items.Count == 0)
                {
                    return NotFound();
                }
                else return Ok(adminDestinations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        [HttpPost("create")]
        public IActionResult CreateNewDestination([FromBody] InputDestinationModel destination)
        {
            try
            {
                int destinationId = _destinationService.AddDestination(destination);
                return CreatedAtAction("Destination created", new { id = destinationId }, destination);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
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