using DaNangTourism.Server.Helper;
using DaNangTourism.Server.Models.DestinationModels;
using DaNangTourism.Server.Models.ReviewModels;
using DaNangTourism.Server.Service;
using Microsoft.AspNetCore.Mvc;

namespace DaNangTourism.Server.Controllers
{
    [ApiController]
    [Route("review")]
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly IAuthenticationHelper _authenticationHelper;

        public ReviewController(IReviewService reviewService, IAuthenticationHelper authenticationHelper)
        {
            _reviewService = reviewService;
            _authenticationHelper = authenticationHelper;
        }

        [HttpGet("list/{id}")]
        public IActionResult GetReviewsByDestinationId([FromRoute] int id, [FromQuery] ReviewFilter reviewFilter)
        {
            reviewFilter.SortBy = DataSanitization.RemoveSpecialCharacters(reviewFilter.SortBy);
            reviewFilter.SortType = DataSanitization.RemoveSpecialCharacters(reviewFilter.SortType);
            try
            {
                var reviews = _reviewService.GetReviewsByDestinationId(id, reviewFilter);
                if (reviews.Items.Count() == 0)
                {
                    return NotFound();
                }
                else return StatusCode(200, new { message = "Success", data = reviews});
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("create")]
        public IActionResult AddReview([FromBody] InputReviewModel review)
        {
            try
            {
                int userId = _authenticationHelper.GetUserIdFromToken();
                _reviewService.AddReview(userId, review);
                return StatusCode(201, "Review created");
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

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteReview([FromRoute] int id)
        {
            try
            {
                // xác thực admin

                // xóa review
                _reviewService.DeleteReview(id);
                return StatusCode(200, "Review deleted");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
