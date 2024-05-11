using Microsoft.AspNetCore.Mvc;
using DaNangTourism.Server.Models;
using DaNangTourism.Server.DAL;

namespace DaNangTourism.Server.Controllers
{
    [ApiController]
    [Route("review")]
    public class ReviewController : Controller
    {
        [HttpGet("get/{id}")]
        public IActionResult GetReviewById([FromRoute] int id)
        {
            Review? review = ReviewDAO.Instance.GetReviewById(id);
            if (review == null)
            {
                return NotFound();
            }
            return Ok(review);
        }
        [HttpGet("get/all")]
        public IActionResult GetReviewsByDesId([FromBody] int id)
        {
            Dictionary<int, Review> reviews = ReviewDAO.Instance.GetReviewsByDesId(id);
            if (reviews.Count == 0)
            {
                return NotFound();
            }    
            else return Ok(reviews);
        }
        [HttpPost("add/{destinationId}")] 
        public IActionResult AddReview([FromBody] AddReviewRequest addReviewRequest, [FromRoute] int destinationId)
        {
            ReviewDAO reviewDAO = ReviewDAO.Instance;
            bool check = reviewDAO.AddReview(addReviewRequest.UserId, destinationId, addReviewRequest.Review) > 0;
            if (check)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPut("update")]
        public IActionResult UpdateReview([FromBody] Review review)
        {
            ReviewDAO reviewDAO = ReviewDAO.Instance;
            bool check = reviewDAO.UpdateReview(review) > 0;
            if (check)
            {
                return Ok();
            }    
            return BadRequest();
        }
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteReview([FromRoute] int id)
        {
            ReviewDAO reviewDAO = ReviewDAO.Instance;
            bool check = reviewDAO.DeleteReview(id) > 0;
            if (check)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
