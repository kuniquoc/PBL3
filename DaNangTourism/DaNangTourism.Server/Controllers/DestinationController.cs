using DaNangTourism.Server.Service;
using DaNangTourism.Server.Helper;
using Microsoft.AspNetCore.Mvc;
using DaNangTourism.Server.Models.DestinationModels;

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
                else return StatusCode(200, new { message = "Success", data = destinations });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        
        [HttpGet("list")]
        public IActionResult GetListDestinations([FromQuery] DestinationFilter destinationFilter)
        {
            // Lọc dữ liệu
            destinationFilter.Sanitization();

            try
            {
                int userId = _authenticationHelper.GetUserIdFromToken();
                var destinations = _destinationService.GetListDestinations(userId, destinationFilter);
                if (destinations.Count() == 0)
                {
                    return NotFound();
                }
                else return StatusCode(200, new { message = "Success", data = destinations });
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
                else return StatusCode(200, new { message = "Success", data = destination });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }
        
        [HttpPut("favorite/{destinationId}")]
        public IActionResult UpdateFavDes([FromRoute] int destinationId, [FromBody] bool favorite)
        {
            try
            {
                int userId = _authenticationHelper.GetUserIdFromToken();
                _destinationService.UpdateFavDes(userId, destinationId, favorite);
                return StatusCode(200, "Favorite updated");
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
        
        [HttpGet("random")]
        public IActionResult GetRandomDestinations([FromQuery] int limit = 3)
        {
            try
            {
                var destinations = _destinationService.GetRandomDestinations(limit);
                if (destinations.Count() == 0)
                {
                    return NotFound();
                }
                else return StatusCode(200, new { message = "Success", data = destinations});
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
       
        [HttpGet("managelist")]
        public IActionResult GetAdminList([FromQuery] AdminDestinationFilter adminDestinationFilter)
        {
            // Lọc dữ liệu
            adminDestinationFilter.Sanitization();

            try
            {
                // xác thực admin


                //
                AdminDestinations adminDestinations = _destinationService.GetDestinationElements(adminDestinationFilter);
                if (adminDestinations.Items.Count == 0)
                {
                    return NotFound();
                }
                else return StatusCode(200, new { message = "Success", data = adminDestinations});
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
                // xác thực admin


                //
                _destinationService.AddDestination(destination);
                return StatusCode(201, new { message = "Destination created" });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        
        [HttpPut("update/{id}")]
        public IActionResult UpdateDestination([FromRoute] int id, [FromBody] InputDestinationModel destination)
        {
            try
            {
                // xác thực admin


                //
                _destinationService.UpdateDestination(id, destination);
                return StatusCode(200, new { message = "Destination updated" });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteDestination([FromRoute] int id)
        {
            try
            {
                // xác thực admin


                //
                _destinationService.DeleteDestination(id);
                return StatusCode(200, new { message = "Destination deleted" });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}