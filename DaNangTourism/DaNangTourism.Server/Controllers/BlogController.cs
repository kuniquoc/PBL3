using DaNangTourism.Server.DAL;
using DaNangTourism.Server.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace DaNangTourism.Server.Controllers
{
    [ApiController]
    [Route("blog")]
    public class BlogController : Controller
    {
        [HttpGet("get/all")]
        public IActionResult GetAllBlog()
        {
            BlogDAO blogDAO = new BlogDAO();
            Dictionary<int, Blog> blogs = blogDAO.GetAllBlog();
            if(blogs.Count == 0)
            {
                return NotFound();
            }
            return Ok(blogs);
        }
        [HttpGet("get/{id}")]
        public IActionResult GetDestinationById(int blogID)
        {
            BlogDAO blogDAO = new BlogDAO();
            Blog blog = blogDAO.GetBlogByBlogID(blogID);
            if (blog == null)
            {
                return NotFound();
            }
            else return Ok(blog);
        }
        [HttpPost("add")]
        public IActionResult AddBlog([FromBody] Blog blog)
        {
            BlogDAO blogDAO = new BlogDAO();
            bool check = blogDAO.AddBlog(blog) > 0;
            if (check)
            {
                return Ok();
            }
            else return BadRequest();
        }
        [HttpPut("update")]
        public IActionResult UpdateDestination([FromBody] Blog blog)
        {
            BlogDAO blogDAO = new BlogDAO();
            bool check = blogDAO.EditBlog(blog) > 0;
            if (check)
            {
                return Ok();
            }
            else return BadRequest();
        }
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteDestination([FromRoute] int blogID)
        {
            BlogDAO blogDAO = new BlogDAO();
            bool check = blogDAO.DeleteBlog(blogID) > 0;
            if (check)
            {
                return Ok();
            }
            else return BadRequest();
        }
    }
}
