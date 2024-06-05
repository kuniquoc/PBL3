using DaNangTourism.Server.Helper;
using DaNangTourism.Server.Models.DestinationModels;
using DaNangTourism.Server.Models.ReviewModels;
using DaNangTourism.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace DaNangTourism.Server.Controllers
{
  [ApiController]
  [Route("review")]
  public class ReviewController : Controller
  {
    private readonly IReviewService _reviewService;
    private readonly IAccountService _accountService;

    public ReviewController(IReviewService reviewService, IAccountService accountService)
    {
      _reviewService = reviewService;
      _accountService = accountService;
    }

    [HttpGet("list/{destinationId}")]
    public IActionResult GetReviewsByDestinationId([FromRoute] int destinationId, [FromQuery] ReviewFilter reviewFilter)
    {
      reviewFilter.SortBy = DataSanitization.RemoveSpecialCharacters(reviewFilter.SortBy);
      reviewFilter.SortType = DataSanitization.RemoveSpecialCharacters(reviewFilter.SortType);
      try
      {
        var reviews = _reviewService.GetReviewsByDestinationId(destinationId, reviewFilter);
        if (reviews.Items.Count == 0)
        {
          return NotFound();
        }
        else return StatusCode(200, new { message = "Success", data = reviews });
      }
      catch (Exception e)
      {
        return StatusCode(500, new { message = e.Message });
      }
    }

    [HttpPost("create")]
    public IActionResult AddReview([FromBody] InputReviewModel review)
    {
      try
      {
        int userId;
        try
        {
          userId = _accountService.GetUserIdFromToken();
        }
        catch (Exception ex)
        {
          return Unauthorized(new { message = ex.Message });
        }

        int id = _reviewService.AddReview(userId, review);
        return StatusCode(201, new { message = "Review created", data = new { id } });
      }
      catch (Exception e)
      {
        return StatusCode(500, new { message = e.Message });
      }
    }

    [HttpDelete("delete/{id}")]
    public IActionResult DeleteReview([FromRoute] int id)
    {
      try
      {
        try
        {
          // xác thực admin
          if (!_accountService.IsAdmin())
          {
            return Unauthorized(new { message = "Can't access it, You aren't admin" });
          }
        }
        catch (Exception ex)
        {
          return Unauthorized(new { message = ex.Message });
        }

        // xóa review
        _reviewService.DeleteReview(id);
        return StatusCode(200, "Review deleted");
      }
      catch (Exception e)
      {
        return StatusCode(500, new { message = e.Message });
      }
    }
  }
}
