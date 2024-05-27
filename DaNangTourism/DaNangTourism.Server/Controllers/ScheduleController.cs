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

        //[HttpGet("sharedlist")]
        
    }
}
