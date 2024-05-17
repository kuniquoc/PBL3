using DaNangTourism.Server.DAL;
using DaNangTourism.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace DaNangTourism.Server.Controllers
{
    [ApiController]
    [Route("favDes")]
    public class FavouriteDestinationController : Controller
    {
        [HttpGet("get/all")] // hiển thị danh sách điểm đến yêu thích
        public IActionResult GetAllFavDesByUserID([FromBody] int userID)
        {
            FavouriteDestinationDAO favDesDAO = FavouriteDestinationDAO.Instance;
            List<int> favDesIds = favDesDAO.GetAllFavouriteDestination(userID);
            if (favDesIds.Count > 0)
            {
                List<Destination> destinations = DestinationDAO.Instance.GetDestinationsByIds(favDesIds);
                return Ok(destinations);
            }
            return NotFound();
        }
        [HttpGet("check/{destinationId}")] // kiểm tra đã thêm điểm đến vào mục yêu thích chưa
        public IActionResult IsFavDes([FromBody] int userId, [FromRoute] int destinationId)
        {
            FavouriteDestinationDAO fDDao = FavouriteDestinationDAO.Instance;
            bool check = fDDao.IsDuplicate(userId, destinationId);
            if (check)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpPost("add/{destinationId}")] // nếu chưa thêm vào mục yêu thích thì mới thêm
        public IActionResult AddFavDes([FromBody] int userId, [FromRoute] int destinationId)
        {
            FavouriteDestinationDAO fDDao = FavouriteDestinationDAO.Instance;
            bool check = fDDao.IsDuplicate(userId, destinationId);
            if (check == false)
            {
                bool check1 = fDDao.AddFavDes(userId, destinationId) > 0;
                if (check1) return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("delete/{destinationId}")]
        public IActionResult DeleteFavDes([FromBody] int userId, [FromRoute] int destinationId)
        {
            FavouriteDestinationDAO fDDao = FavouriteDestinationDAO.Instance;
            bool check = fDDao.AddFavDes(userId, destinationId) > 0;
            if (check) return Ok();
            return BadRequest();
        }
        
    }
}
