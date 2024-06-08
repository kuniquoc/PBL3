using DaNangTourism.Server.DAL;
using DaNangTourism.Server.Services;
using DaNangTourism.Server.Helper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;
using DaNangTourism.Server.Models.ScheduleModels;

namespace DaNangTourism.Server.Controllers
{
  [ApiController]
  [Route("schedule")]

  public class ScheduleController : Controller
  {
    private readonly IScheduleService _scheduleService;
    private readonly IAccountService _accountService;
    public ScheduleController(IScheduleService scheduleService, IAccountService accountService)
    {
      _scheduleService = scheduleService;
      _accountService = accountService;
    }
    [HttpGet("mySchedule")]
    public IActionResult GetAllSchedules([FromQuery] ScheduleFilter scheduleFilter)
    {
      // khử dữ liệu lọc
      scheduleFilter.Sanitization();
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

        var schedules = _scheduleService.GetListSchedule(userId, scheduleFilter);
        if (schedules.Items.Count == 0)
        {
          return NotFound();
        }
        return StatusCode(200, new { message = "Success", data = schedules });
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
      }
    }

    [HttpGet("sharedSchedule")]
    public IActionResult GetSharedSchedules([FromQuery] PublicScheduleFilter scheduleFilter)
    {
      scheduleFilter.Sanitization();
      try
      {
        int userId;
        try
        {
          userId = _accountService.GetUserIdFromToken();
        }
        catch (Exception)
        {
          userId = 0;
        }

        var schedules = _scheduleService.GetPublicSchedule(scheduleFilter, userId);
        if (schedules.Items.Count == 0)
        {
          return NotFound();
        }
        return StatusCode(200, new { message = "Success", data = schedules });
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
      }
    }

    [HttpGet("detail/{id}")]
    public IActionResult GetScheduleDetail([FromRoute] int id)
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

        var schedule = _scheduleService.GetScheduleDetail(userId, id);
        if (schedule == null)
        {
          return NotFound();
        }
        return StatusCode(200, new { message = "Success", data = schedule });
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
      }
    }

    [HttpPost("create")]
    public IActionResult CreateSchedule([FromBody] InputSchedule schedule)
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

        // lấy name từ token
        string name = _accountService.GetUserName();

        var id = _scheduleService.CreateSchedule(userId, name, schedule);
        if (id == 0)
        {
          return BadRequest();
        }
        return StatusCode(200, new { message = "Create schedule successful", data = new { id } });
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
      }
    }

    [HttpPost("clone/{scheduleId}")]
    public IActionResult CloneSchedule([FromRoute] int scheduleId)
    {
      try
      {
        int userId;
        string name;
        try
        {
          userId = _accountService.GetUserIdFromToken();
          name = _accountService.GetUserName();
        }
        catch (Exception ex)
        {
          return Unauthorized(new { message = ex.Message });
        }

        var id = _scheduleService.CloneSchedule(userId, name, scheduleId);
        if (id == 0)
        {
          return BadRequest();
        }
        return StatusCode(200, new { message = "Clone schedule successful", data = new { id } });
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
      }
    }

    [HttpPost("addDestination")]
    public IActionResult AddScheduleDestination([FromBody] AddScheduleDestinationModel scheduleDestination)
    {
      if (scheduleDestination.ArrivalTime >= scheduleDestination.LeaveTime)
        return BadRequest();
      try
      {
        try
        {
          _accountService.GetUserIdFromToken();
        }
        catch (Exception ex)
        {
          return Unauthorized(new { message = ex.Message });
        }

        var id = _scheduleService.AddScheduleDestination(scheduleDestination);

        return StatusCode(200, new { message = "Add destination to schedule successful", data = new { id } });
      }
      catch (BadHttpRequestException e)
      {
        return BadRequest(new { message = e.Message });
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
      }
    }

    [HttpDelete("removeDestination/{scheduleDestinationId}")]
    public IActionResult RemoveScheduleDestination([FromRoute] int scheduleDestinationId)
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

        _scheduleService.DeleteScheduleDestination(userId, scheduleDestinationId);
        return StatusCode(200, new { message = "Remove destination from schedule successful" });
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
      }
    }

    [HttpPut("updateDestination/{scheduleDestinationId}")]
    public IActionResult UpdateScheduleDestination([FromRoute] int scheduleDestinationId, [FromBody] UpdateScheduleDestinationModel scheduleDestination)
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

        _scheduleService.UpdateScheduleDestination(userId, scheduleDestinationId, scheduleDestination);
        return StatusCode(200, new { message = "Update destination in schedule successful" });
      }
      catch (BadHttpRequestException e)
      {
        return BadRequest(new { message = e.Message });
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
      }
    }


    [HttpPut("update/{scheduleId}")]
    public IActionResult UpdateSchedule([FromRoute] int scheduleId, [FromBody] UpdateScheduleModel schedule)
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

        _scheduleService.UpdateSchedule(userId, scheduleId, schedule);

        return StatusCode(200, new { message = "Update schedule successful" });
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
      }
    }

    [HttpDelete("delete/{scheduleId}")]
    public IActionResult DeleteSchedule([FromRoute] int scheduleId)
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

        _scheduleService.DeleteSchedule(userId, scheduleId);
        return StatusCode(200, new { message = "Delete schedule successful" });
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
      }
    }
  }
}
