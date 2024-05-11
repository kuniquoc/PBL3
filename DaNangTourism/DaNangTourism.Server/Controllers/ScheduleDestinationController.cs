using DaNangTourism.Server.DAL;
using DaNangTourism.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace DaNangTourism.Server.Controllers
{
    [ApiController]
    [Route("sd")]
    public class ScheduleDestinationController:Controller
    {
        [HttpGet("get/all")]
        public IActionResult GetSDsByScheduleId([FromBody] int scheduleId)
        {
            ScheduleDestinationDAO sDDAO = ScheduleDestinationDAO.Instance;
            Dictionary<int, ScheduleDestination> sDs = sDDAO.GetSDsByScheduleID(scheduleId);
            if (sDs.Count == 0)
            {
                return NotFound();
            }
            return Ok(sDs);
        }
        [HttpGet("get/{id}")]
        public IActionResult GetSDById([FromRoute] int id)
        {
            ScheduleDestinationDAO sDDAO = ScheduleDestinationDAO.Instance;
            ScheduleDestination? sD = sDDAO.GetSDById(id);
            if (sD == null)
            {
                return NotFound();
            }
            return Ok(sD);
        }
        [HttpPost("add")] 
        public IActionResult AddSD([FromBody] AddSDRequest addSDRequest)
        {
            ScheduleDestinationDAO sDDAO = ScheduleDestinationDAO.Instance;
            bool check = sDDAO.AddSD(addSDRequest.ScheduleId, addSDRequest.DestinationId, addSDRequest.SD) > 0;
            if (check)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPut("update")]
        public IActionResult UpdateSD([FromBody] ScheduleDestination sD)
        {
            ScheduleDestinationDAO sDDAO = ScheduleDestinationDAO.Instance;
            bool check = sDDAO.UpdateSD(sD) > 0;
            if (check)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteSD([FromRoute] int id)
        {
            ScheduleDestinationDAO sDDAO = ScheduleDestinationDAO.Instance;
            bool check = sDDAO.DeleteSD(id) > 0;
            if (check)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
