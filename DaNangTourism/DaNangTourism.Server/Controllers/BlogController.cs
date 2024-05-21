using DaNangTourism.Server.BLL;
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
        [HttpGet("home")]
        public IActionResult get5MostView()
        {
            List<BlogHome> blogHomes = BlogBLL.Instance.get5MostView();
            if(blogHomes.Count == 0)
            {
                return NotFound();
            }
            return Ok(blogHomes);
        }

        [HttpGet("blogPage")]
        public IActionResult getAllBlog(IQueryCollection query)
        {
            List<BlogPage> blogPages = BlogBLL.Instance.getBlogPage(query);
            if(blogPages.Count == 0)
            {
                return NotFound();
            }
            return Ok(blogPages);
        }
        [HttpGet("random")]
        public IActionResult getRandomBlog(IQueryCollection query)
        {
            List<BlogRandom> blogRandoms = BlogBLL.Instance.getRandomBlog(query);
            if (blogRandoms.Count == 0)
            {
                return NotFound();
            }
            return Ok(blogRandoms);
        }
        [HttpGet("blogDetail/{id}")]
        public IActionResult getBlogDetail([FromRoute]int id)
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
        [HttpPost("create")]
        public IActionResult CreateNewDestination(IQueryCollection query)
        {
            return Ok();
        }
        [HttpPut("update/{id}")]
        public IActionResult UpdateDestination([FromRoute] int id)
        {
            return Ok();
        }
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteDestination([FromRoute] int id)
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
