using DaNangTourism.Server.Services;
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
    private readonly IAccountService _accountService;
    public DestinationController(IDestinationService destinationService, IAccountService accountService)
    {
      _destinationService = destinationService;
      _accountService = accountService;
    }

    [HttpGet("home")]
    public IActionResult GetHomeDestinations()
    {
      try
      {
        var destinations = _destinationService.GetHomeDestinations();
        if (destinations.Count() == 0)
        {
          return NotFound(new { message = "No destination found" });
        }
        else return StatusCode(200, new { message = "Success", data = destinations });
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
      }
    }

    [HttpGet("list")]
    public IActionResult GetListDestinations([FromQuery] DestinationFilter destinationFilter)
    {
      destinationFilter.Sanitization();
      try
      {
        int userId;
        try
        {
          userId = _accountService.GetUserIdFromToken();
        }
        catch (Exception ex)
        {
          userId = 0;
        }

        var destinations = _destinationService.GetListDestinations(destinationFilter, userId);
        if (destinations.Items.Count() == 0)
        {
          return NotFound(new { message = "No destination found" });
        }
        else return StatusCode(200, new { message = "Success", data = destinations });
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
      }
    }

    [HttpGet("detail/{id}")]
    public IActionResult GetDestinationById([FromRoute] int id)
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
          userId = 0;
        }

        var destination = _destinationService.GetDestinationDetail(id, userId);
        if (destination == null)
        {
          return NotFound(new { message = "No destination found" });
        }
        else return StatusCode(200, new { message = "Success", data = destination });
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
      }

    }

    [HttpPut("favorite")]
    public IActionResult UpdateFavDes([FromBody] FavoriteModel favorite)
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

        _destinationService.UpdateFavDes(userId, favorite.DestinationId, favorite.IsFavorite);
        return StatusCode(200, "Favorite updated");
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
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
          return NotFound(new { message = "No destination found" });
        }
        else return StatusCode(200, new { message = "Success", data = destinations });
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
      }
    }

    [HttpGet("managelist")]
    public IActionResult GetAdminList([FromQuery] AdminDestinationFilter adminDestinationFilter)
    {
      // Lọc dữ liệu
      adminDestinationFilter.Sanitization();

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

        AdminDestinations adminDestinations = _destinationService.GetDestinationElements(adminDestinationFilter);
        if (adminDestinations.Items.Count == 0)
        {
          return NotFound(new { message = "No destination found" });
        }
        else return StatusCode(200, new { message = "Success", data = adminDestinations });
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
      }
    }

    [HttpPost("create")]
    public IActionResult CreateNewDestination([FromBody] InputDestinationModel destination)
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

        int id = _destinationService.AddDestination(destination);
        return StatusCode(201, new { message = "Destination created", data = new { id } });
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
      }
    }
    [HttpGet("GetToUpdate/{id}")]
    public IActionResult GetDestinationToUpdate([FromRoute] int id)
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

        var returnDes = _destinationService.GetDestinationToUpdate(id);
        return StatusCode(200, new { message = "Get successful", data = returnDes });
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
      }
    }

    [HttpPut("update/{id}")]
    public IActionResult UpdateDestination([FromRoute] int id, [FromBody] InputDestinationModel destination)
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

        var returnDes = _destinationService.UpdateDestination(id, destination);
        return StatusCode(200, new { message = "Destination updated", data = returnDes });
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
      }
    }

    [HttpDelete("delete/{id}")]
    public IActionResult DeleteDestination([FromRoute] int id)
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
          Console.WriteLine(ex.Message);
          return Unauthorized(new { message = "Error occurred while authenticating, please try again later." });
        }

        _destinationService.DeleteDestination(id);
        return StatusCode(200, new { message = "Destination deleted" });
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
      }
    }
  }
}