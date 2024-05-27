using DaNangTourism.Server.DAL;
using DaNangTourism.Server.Service;
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
        private readonly IAuthenticationHelper _authenticationHelper;
        public ScheduleController(IScheduleService scheduleService, IAuthenticationHelper authenticationHelper)
        {
            _scheduleService = scheduleService;
            _authenticationHelper = authenticationHelper;
        }
        [HttpGet("myschedule")]
        public IActionResult GetAllSchedules([FromQuery] ScheduleFilter scheduleFilter)
        {
            // khử dữ liệu lọc
            scheduleFilter.Sanitization();
            try
            {
                int userId = _authenticationHelper.GetUserIdFromToken();
                var schedules = _scheduleService.GetListSchedule(userId, scheduleFilter);
                if (schedules.Items.Count() == 0)
                {
                    return NotFound();
                }
                else
                return StatusCode(200, new {message = "Success", data = schedules});
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

        [HttpGet("sharedlist")]
        public IActionResult GetSharedSchedules([FromQuery] PublicScheduleFilter scheduleFilter)
        {
            scheduleFilter.Sanitization();
            try
            {
                int userId = _authenticationHelper.GetUserIdFromToken();
                var schedules = _scheduleService.GetPublicSchedule(scheduleFilter, userId);
                if (schedules.Items.Count() == 0)
                {
                    return NotFound();
                }
                else
                    return StatusCode(200, new { message = "Success", data = schedules });
            }
            catch (UnauthorizedAccessException)
            {
                var schedules = _scheduleService.GetPublicSchedule(scheduleFilter, 0);
                if (schedules.Items.Count() == 0)
                {
                    return NotFound();
                }
                else
                    return StatusCode(200, new { message = "Success", data = schedules });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public IActionResult GetScheduleDetail([FromQuery] int id)
        {
            try
            {
                int userId = _authenticationHelper.GetUserIdFromToken();
                var schedule = _scheduleService.GetScheduleDetail(userId, id);
                if (schedule == null)
                {
                    return NotFound();
                }
                else
                    return StatusCode(200, new { message = "Success", data = schedule });
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
        
        [HttpPost("create")]    
        public IActionResult CreateSchedule([FromBody] InputSchedule schedule)
        {
            try
            {
                int userId = _authenticationHelper.GetUserIdFromToken();
                // lấy name từ token
                string name = "";

                var id = _scheduleService.CreateSchedule(userId, name, schedule);
                if (id == 0)
                {
                    return BadRequest();
                }
                else
                    return StatusCode(200, new { message = "Create schedule successful", data = id });
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

        [HttpPost("clone/{scheduleId}")]
        public IActionResult CloneSchedule([FromRoute] int scheduleId)
        {
            try
            {
                int userId = _authenticationHelper.GetUserIdFromToken();
                // lấy name từ token
                string name = "";

                var id = _scheduleService.CloneSchedule(userId, name, scheduleId);
                if (id == 0)
                {
                    return BadRequest();
                }
                else
                    return StatusCode(200, new { message = "Clone schedule successful", data = id });
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

        [HttpPost("adddestination")]
        public IActionResult AddScheduleDestination([FromBody] ScheduleDestination scheduleDestination)
        {
            try
            {
                _authenticationHelper.GetUserIdFromToken();

                var id = _scheduleService.AddScheduleDestination(scheduleDestination);
                if (id == 0)
                {
                    return BadRequest();
                }
                else
                    return StatusCode(200, new { message = "Add destination to schedule successful", data = id });
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
        
        [HttpDelete("removedestination/{scheduleDestinationId}")]        
        public IActionResult RemoveScheduleDestination([FromRoute] int scheduleDestinationId)
        {
            try
            {
                int userId = _authenticationHelper.GetUserIdFromToken();

                _scheduleService.DeleteScheduleDestination(userId, scheduleDestinationId);
                 return StatusCode(200, new { message = "Remove destination from schedule successful" });
            }
            catch (UnauthorizedAccessException uae)
            {
                return Unauthorized(uae.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
