using DaNangTourism.Server.Services;
using DaNangTourism.Server.DAL;
using DaNangTourism.Server.Models;
using Microsoft.AspNetCore.Mvc;
using DaNangTourism.Server.Models.DestinationModels;
using System.Security.Principal;


namespace DaNangTourism.Server.Controllers
{
    [ApiController]
    [Route("blog")]
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly IAccountService _accountService;
        public BlogController(IBlogService blogService, IAccountService accountService)
        {
            _blogService = blogService;
            _accountService = accountService;
        }

        [HttpGet("home")]
        public IActionResult Get5MostView()
        {
            try
            {
                List<BlogHome>? blogHomes = _blogService.Get5MostView();

                if (blogHomes == null || blogHomes.Count == 0)
                {
                    return NotFound();
                }
                BlogReturn<List<BlogHome>> blogReturn = new BlogReturn<List<BlogHome>>(blogHomes);
                return Ok(blogReturn);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Server Error");
            }
        }

        [HttpGet("blogPage")]
        public IActionResult GetAllBlog([FromQuery] BlogPageFilter blogPageFilter)
        {
            try
            {
                var blogPageData = _blogService.GetBlogPage(blogPageFilter);
                if (blogPageData.items.Count == 0)
                {
                    return NotFound();
                }
                BlogReturn<BlogPageData> blogReturn = new BlogReturn<BlogPageData>(blogPageData);
                return Ok(blogReturn);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Server Error");
            }
        }

        [HttpGet("random")]
        public IActionResult GetRandomBlog([FromQuery] BlogRandomFilter blogRandomFilter)
        {
            try
            {
                List<BlogRandom> blogRandoms = _blogService.GetRandomBlog(blogRandomFilter);
                if (blogRandoms.Count == 0)
                {
                    return NotFound();
                }
                BlogReturn<List<BlogRandom>> blogReturn = new BlogReturn<List<BlogRandom>>(blogRandoms);
                return Ok(blogRandoms);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Server Error");
            }
        }

        [HttpGet("blogDetail/{id}")]
        public IActionResult GetBlogDetail([FromRoute] int id)
        {
            try
            {
                BlogDetail? blogDetail = _blogService.GetBlogDetail(id);
                if (blogDetail == null)
                {
                    return NotFound();
                }
                else
                {
                    _blogService.IncreaseView(id);
                    BlogReturn<BlogDetail> blogReturn = new BlogReturn<BlogDetail>(blogDetail);
                    return Ok(blogReturn);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Server Error");
            }
        }

        [HttpPost("create")]
        public IActionResult CreateNewBlog([FromBody] BlogAdd blogAdd)
        {
            try
            {
                // nhận id
                int uid = _accountService.GetUserIdFromToken();

                // nhận id blog mới thêm
                int id = _blogService.AddBlog(blogAdd, uid);

                return StatusCode(200, new { message = "Success", data = new { id } });
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

        [HttpPut("update/{id}")]
        public IActionResult UpdateBlog([FromRoute] int id, [FromBody] BlogAdd blogAdd)
        {
            try
            {
                // nhận id
                int uid = _accountService.GetUserIdFromToken();

                if (_blogService.CheckBlogBelongToUser(id, uid))
                {
                    var returnBlog = _blogService.UpdateBlog(blogAdd, id);
                    if (returnBlog == null)
                    {
                        return BadRequest(new { message = "Update fail; Don't exist this blog" });
                    }
                    return Ok(new { message = "Success", data = returnBlog });
                }
                else
                {
                    return Unauthorized(new { message = "You are not Author" });
                }
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
        public IActionResult deleteBlog([FromRoute] int id)
        {
            try
            {
                // nhận id
                int uid = _accountService.GetUserIdFromToken();

                if (_accountService.IsAdmin())
                {
                    _blogService.DeleteBlog(id);
                    return Ok(new { message = "Success" });
                }
                else
                {
                    if (_blogService.CheckBlogBelongToUser(id, uid))
                    {
                        _blogService.DeleteBlog(id);
                        return Ok(new { message = "Success" });
                    }
                    else
                    {
                        return StatusCode(500, "You are not Author");
                    }
                }
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
        
        //[HttpGet("managelist")]
        //public IActionResult getBlogList([FromQuery] BlogListAdminFilter blogListAdminFilter)
        //{
        //    try
        //    {
        //        // nhận id
        //        int uid = _accountService.GetUserIdFromToken();

        //        if (_accountService.IsAdmin())
        //        {
        //            List<BlogList> blogLists = _blogService.GetBlogList(blogListAdminFilter);
        //            BLogListData blogListData = new BLogListData(blogLists, blogListAdminFilter);
        //            BlogReturn<BLogListData> blogReturn = new BlogReturn<BLogListData>(blogListData);
        //            return Ok(blogReturn);
        //        }
        //        else
        //        {
        //            return StatusCode(500, "You are not Admin");
        //        }
        //    }
        //    catch (UnauthorizedAccessException)
        //    {
        //        return Unauthorized();
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpPut("updateStatus/{id}")]
        //public IActionResult updateStatus([FromRoute] int id, [FromRoute] Status status)
        //{
        //    if (!HttpContext.Request.Cookies.ContainsKey("token"))
        //    {
        //        return Unauthorized();
        //    }
        //    else
        //    {
        //        try
        //        {
        //            var claims = accountBLL.GetClaimsByCookie(HttpContext);
        //            var uid = Convert.ToInt32(claims["id"]);
        //            var account = AccountRepository.Instance.GetAccountById(uid);
        //            if (account.Permission == Permission.admin)
        //            {
        //                _blogService.updateStatus(id, status);
        //                return StatusCode(200, "Success");
        //            }
        //            else
        //            {
        //                return StatusCode(500, "You are not Admin");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //            return StatusCode(500, "Server Error");
        //        }
        //    }
        //}
    }
}
