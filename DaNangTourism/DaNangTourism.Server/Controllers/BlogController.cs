using DaNangTourism.Server.BLL;
using DaNangTourism.Server.DAL;
using DaNangTourism.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

namespace DaNangTourism.Server.Controllers
{
    [ApiController]
    [Route("blog")]
    public class BlogController : Controller
    {
        private readonly IConfiguration _configuration;
        private AccountBLL accountBLL;
        public BlogController(IConfiguration configuration)
        {
            _configuration = configuration;
            accountBLL = AccountBLL.Instance(_configuration);
        }
        [HttpGet("home")]
        public IActionResult get5MostView()
        {
            try
            {
                List<BlogHome>? blogHomes = BlogBLL.Instance.get5MostView();

                if (blogHomes == null ||blogHomes.Count == 0)
                {
                    return NotFound();
                }
                return Ok(blogHomes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Server Error");
            }
        }

        [HttpGet("blogPage")]
        public IActionResult getAllBlog([FromQuery] BlogPageFilter blogPageFilter)
        {
            try
            {
                List<BlogPage> blogPages = BlogBLL.Instance.getBlogPage(blogPageFilter);
                if (blogPages.Count == 0)
                {
                    return NotFound();
                }
                return Ok(blogPages);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Server Error");
            }
        }
        [HttpGet("random")]
        public IActionResult getRandomBlog([FromQuery] BlogRandomFilter blogRandomFilter)
        {
            try
            {
                List<BlogRandom> blogRandoms = BlogBLL.Instance.getRandomBlog(blogRandomFilter);
                if (blogRandoms.Count == 0)
                {
                    return NotFound();
                }
                return Ok(blogRandoms);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Server Error");
            }
        }
        [HttpGet("blogDetail/{id}")]
        public IActionResult getBlogDetail([FromRoute]int id)
        {
            try
            {
                BlogDetail blogDetail = BlogBLL.Instance.getBlogDetail(id);
                if (blogDetail == null)
                {
                    return NotFound();
                }
                else
                {
                    BlogBLL.Instance.increaseView(id);
                    return Ok(blogDetail);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Server Error");
            }
        }
        [HttpPost("create")]
        public IActionResult createNewBlog([FromBody] BlogAdd blogAdd)
        {
            int uid = 0;
            if (!HttpContext.Request.Cookies.ContainsKey("token"))
            {
                //xác thực token và nhận id
                return Unauthorized();
            }
            else
            {
                try
                {
                    var claims = accountBLL.GetClaimsByCookie(HttpContext);
                    uid = Convert.ToInt32(claims["id"]);
                    int blogId = BlogBLL.Instance.addBlog(blogAdd, uid);
                    return CreatedAtAction("Success", new { id = blogId }, blogAdd);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return StatusCode(500, "Server error");
                }
            }
        }
        [HttpPut("update/{id}")]
        public IActionResult updateBlog([FromRoute] int id, [FromBody] BlogAdd blogAdd)
        {
            if (!HttpContext.Request.Cookies.ContainsKey("token"))
            {
                return Unauthorized();
            }
            else
            {
                try
                {
                    var claims = accountBLL.GetClaimsByCookie(HttpContext);
                    int uid = Convert.ToInt32(claims["id"]);
                    if (BlogBLL.Instance.checkBlogBelongToUser(id, uid) == 1)
                    {
                        BlogBLL.Instance.updateBlog(blogAdd, id);
                        return StatusCode(200, "Success");
                    }
                    else
                    {
                        return StatusCode(500, "You are not Author");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return StatusCode(500, "Server Error");
                }
            }
        }
        [HttpDelete("delete/{id}")]
        public IActionResult deleteBlog([FromRoute] int id)
        {
            if (!HttpContext.Request.Cookies.ContainsKey("token"))
            {
                return Unauthorized();
            }
            else
            {
                try
                {
                    var claims = accountBLL.GetClaimsByCookie(HttpContext);
                    int uid = Convert.ToInt32(claims["id"]);
                    var account = AccountDAO.Instance.GetAccountById(uid);

                    if (account.Permission == Permission.admin)
                    {
                        BlogBLL.Instance.deleteBlog(id);
                        return StatusCode(200, "Success");
                    }
                    else
                    {
                        if (BlogBLL.Instance.checkBlogBelongToUser(id, uid) == 1)
                        {
                            BlogBLL.Instance.deleteBlog(id);
                            return StatusCode(200, "Success");
                        }
                        else
                        {
                            return StatusCode(500, "You are not Author");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return StatusCode(500, "Server Error");
                }
            }
        }
        [HttpGet("blogList")]
        public IActionResult getBlogList([FromQuery] BlogListAdminFilter blogListAdminFilter)
        {
            if(!HttpContext.Request.Cookies.ContainsKey("token"))
            {
                return Unauthorized();
            }
            else
            {
                try
                {
                    var claims = accountBLL.GetClaimsByCookie(HttpContext);
                    var uid = Convert.ToInt32(claims["id"]);
                    var account = AccountDAO.Instance.GetAccountById(uid);
                    if(account.Permission == Permission.admin)
                    {
                        List<BlogList> blogLists = BlogBLL.Instance.GetBlogList(blogListAdminFilter);
                        return Ok(blogLists);
                    }
                    else
                    {
                        return StatusCode(500, "You are not Admin");
                    }
                }
                catch(Exception ex) 
                {
                    Console.WriteLine(ex.Message);
                    return StatusCode(500, "Server Error");
                }
            }
        }

        [HttpPut("updateStatus/{id}")]
        public IActionResult updateStatus([FromRoute] int id, [FromRoute] Status status)
        {
            if (!HttpContext.Request.Cookies.ContainsKey("token"))
            {
                return Unauthorized();
            }
            else
            {
                try 
                {
                    var claims = accountBLL.GetClaimsByCookie(HttpContext);
                    var uid = Convert.ToInt32(claims["id"]);
                    var account = AccountDAO.Instance.GetAccountById(uid);
                    if(account.Permission == Permission.admin)
                    {
                        BlogBLL.Instance.updateStatus(id, status);
                        return StatusCode(200, "Success");
                    }
                    else
                    {
                        return StatusCode(500, "You are not Admin");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return StatusCode(500, "Server Error");
                }
            }
        }
        //    [HttpPost("add")]
        //    public IActionResult AddBlog([FromBody] Blog blog)
        //    {
        //        BlogDAO blogDAO = new BlogDAO();
        //        bool check = blogDAO.AddBlog(blog) > 0;
        //        if (check)
        //        {
        //            return Ok();
        //        }
        //        else return BadRequest();
        //    }
        //    [HttpPut("update")]
        //    public IActionResult UpdateDestination([FromBody] Blog blog)
        //    {
        //        BlogDAO blogDAO = new BlogDAO();
        //        bool check = blogDAO.EditBlog(blog) > 0;
        //        if (check)
        //        {
        //            return Ok();
        //        }
        //        else return BadRequest();
        //    }
        //    [HttpDelete("delete/{blogID}")]
        //    public IActionResult DeleteDestination([FromRoute] int blogID)
        //    {
        //        BlogDAO blogDAO = new BlogDAO();
        //        bool check = blogDAO.DeleteBlog(blogID) > 0;
        //        if (check)
        //        {
        //            return Ok();
        //        }
        //        else return BadRequest();
        //    }
    }
}
