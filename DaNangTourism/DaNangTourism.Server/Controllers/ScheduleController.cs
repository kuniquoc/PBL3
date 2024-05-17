using DaNangTourism.Server.DAL;
using Microsoft.AspNetCore.Mvc;
using DaNangTourism.Server.Models;
using System.Diagnostics.Eventing.Reader;

namespace DaNangTourism.Server.Controllers
{
    [ApiController]
    [Route("schedule")]

    public class ScheduleController : Controller
    {
        [HttpGet("get/all")]
        public IActionResult GetAllSchedules(string userId)
        {
            ScheduleDAO scheduleDAO = ScheduleDAO.Instance;
            List<Schedule> schedules = scheduleDAO.GetAllSchedule();
            if (schedules.Count == 0)
            {
                return NotFound();
            }
            return Ok(schedules);
        }
        [HttpGet("get/userId")]
        public IActionResult GetSchedulesByUserId([FromBody] int userId) { 
            ScheduleDAO scheduleDAO = ScheduleDAO.Instance;
            List<Schedule> schedules = scheduleDAO.GetSchedulesByUserId(userId);
            if (schedules.Count == 0)
            {
                return NotFound();
            }
            return Ok(schedules);
        }
        [HttpGet("get/{id}")]
        public IActionResult GetScheduleById([FromRoute] int id)
        {
            ScheduleDAO scheduleDAO = ScheduleDAO.Instance;
            Schedule? schedule = scheduleDAO.GetScheduleById(id);
            if (schedule == null)
            {
                return NotFound();
            }
            return Ok(schedule);
        }
        [HttpPost("add")]
        public IActionResult AddSchedule([FromBody] AddScheduleRequest addScheduleRequest)
        {
            ScheduleDAO scheduleDAO = ScheduleDAO.Instance;
            bool check = scheduleDAO.AddSchedule(addScheduleRequest.UserId, addScheduleRequest.Schedule) > 0;
            if (check)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPut("update")]
        public IActionResult UpdateSchedule([FromBody] Schedule schedule)
        {
            ScheduleDAO scheduleDAO= ScheduleDAO.Instance;
            bool check = scheduleDAO.UpdateSchedule(schedule) > 0;
            if (check)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteSchedule([FromRoute] int id)
        {
            ScheduleDAO scheduleDAO = ScheduleDAO.Instance;
            bool check = scheduleDAO.DeleteSchedule(id) > 0;
            if (check)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
