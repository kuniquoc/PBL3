using DaNangTourism.Server.BLL;
using DaNangTourism.Server.DAL;
using DaNangTourism.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace DaNangTourism.Server.Controllers
{
    [ApiController]
    [Route("blog")]
    public class BlogController : Controller
    {
        [HttpGet("home")]
        public IActionResult get5MostView()
        {
            try
            {
                List<BlogHome> blogHomes = BlogBLL.Instance.get5MostView();
                if (blogHomes.Count == 0)
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
        public IActionResult getAllBlog(IQueryCollection query)
        {
            try
            {
                List<BlogPage> blogPages = BlogBLL.Instance.getBlogPage(query);
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
        public IActionResult getRandomBlog(IQueryCollection query)
        {
            try
            {
                List<BlogRandom> blogRandoms = BlogBLL.Instance.getRandomBlog(query);
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
                    BlogDAO.Instance.increaseView(id);
                    return Ok(blogDetail);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Server Error");
            }
        }
        [Authorize]
        [HttpPost("create")]
        public IActionResult CreateNewBlog(IQueryCollection query)
        {
            string? token;
            if (HttpContext.Request.Cookies.TryGetValue("token", out token))
            {
                //xác thực token và nhận id

            }
            return Ok();
        }
        [HttpPut("update/{id}")]
        public IActionResult UpdateBlog([FromRoute] int id)
        {
            return Ok();
        }
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteBlog([FromRoute] int id)
        {
            return Ok();
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
