using Microsoft.AspNetCore.Mvc;
using DaNangTourism.Server.Models;

namespace DaNangTourism.Server.Controllers
{
    [ApiController]
    [Route("review")]
    public class ReviewController : Controller
    {

        [HttpGet("add/{id}")]
        public IActionResult AddReview(int id, [FromBody] Review review)
        {

            return Ok();
        }
        
    }
}
