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
          return NotFound(new { message = "No blog found" });
        }
        var blogReturn = new BlogReturn<List<BlogHome>>(blogHomes);
        return Ok(blogReturn);
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
      }
    }

    [HttpGet("list")]
    public IActionResult GetAllBlog([FromQuery] BlogPageFilter blogPageFilter)
    {
      try
      {
        var blogPageData = _blogService.GetBlogPage(blogPageFilter);
        if (blogPageData.items.Count == 0)
        {
          return NotFound(new { message = "No blog found" });
        }
        var blogReturn = new BlogReturn<BlogPageData>(blogPageData);
        return Ok(blogReturn);
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
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
          return NotFound(new { message = "No blog found" });
        }
        var blogReturn = new BlogReturn<List<BlogRandom>>(blogRandoms);
        return Ok(blogReturn);
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
      }
    }

    [HttpGet("detail/{id}")]
    public IActionResult GetBlogDetail([FromRoute] int id)
    {
      try
      {
        int uid;
        try
        {
          uid = _accountService.GetUserIdFromToken();
        }
        catch (Exception ex)
        {
          uid = 0;
        }
        bool isAdmin = _accountService.IsAdmin();
        BlogDetail? blogDetail = null;

        blogDetail = _blogService.GetBlogDetail(id, isAdmin, uid);
        if (blogDetail == null)
        {
          return NotFound(new { message = "Blog not found" });
        }
        _blogService.IncreaseView(id);
        var blogReturn = new BlogReturn<BlogDetail>(blogDetail);
        return Ok(blogReturn);
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
      }
    }

    [HttpPost("create")]
    public IActionResult CreateNewBlog([FromBody] BlogAdd blogAdd)
    {
      try
      {
        int uid;
        try
        {
          uid = _accountService.GetUserIdFromToken();
        }
        catch (Exception ex)
        {
          return Unauthorized(new { message = ex.Message });
        }

        // nhận id blog mới thêm
        int id = _blogService.AddBlog(blogAdd, uid);

        return StatusCode(200, new { message = "Success", data = new { id } });
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
      }
    }

    [HttpGet("GetToUpdate/{id}")]
    public IActionResult GetToUpdateBlog(int id)
    {
      try
      {
        int uid;
        try
        {
          uid = _accountService.GetUserIdFromToken();
        }
        catch (Exception ex)
        {
          return Unauthorized(new { message = ex.Message });
        }

        // nhận blog bằng id
        var returnBlog = _blogService.GetBlogToUpdate(id);

        return StatusCode(200, new { message = "Get successful", data = returnBlog });
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
      }
    }

    [HttpPut("update/{id}")]
    public IActionResult UpdateBlog([FromRoute] int id, [FromBody] BlogAdd blogAdd)
    {
      try
      {
        int uid;
        try
        {
          uid = _accountService.GetUserIdFromToken();
        }
        catch (Exception ex)
        {
          return Unauthorized(new { message = ex.Message });
        }

        if (_blogService.CheckBlogBelongToUser(id, uid))
        {
          _blogService.UpdateBlog(blogAdd, id);
          return Ok(new { message = "Success" });
        }
        else
        {
          return Unauthorized(new { message = "You are not Author" });
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
      }
    }

    [HttpDelete("delete/{id}")]
    public IActionResult DeleteBlog([FromRoute] int id)
    {
      try
      {
        int uid;
        try
        {
          uid = _accountService.GetUserIdFromToken();
        }
        catch (Exception ex)
        {
          return Unauthorized(new { message = ex.Message });
        }

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
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
      }
    }

    [HttpGet("managelist")]
    public IActionResult GetBlogList([FromQuery] BlogListAdminFilter blogListAdminFilter)
    {
      try
      {
        int uid;
        try
        {
          uid = _accountService.GetUserIdFromToken();
        }
        catch (Exception ex)
        {
          return Unauthorized(new { message = ex.Message });
        }

        if (!_accountService.IsAdmin())
        {
          return Unauthorized(new { message = "You are not Admin" });
        }

        var blogListData = _blogService.GetBlogList(blogListAdminFilter);
        var blogReturn = new BlogReturn<BLogListData>(blogListData);
        return Ok(blogReturn);
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
      }
    }

    [HttpGet("myBlogs")]
    public IActionResult GetMyBlogs([FromQuery] BlogListAdminFilter blogListAdminFilter)
    {
      try
      {
        int uid;
        try
        {
          uid = _accountService.GetUserIdFromToken();
        }
        catch (Exception ex)
        {
          return Unauthorized(new { message = ex.Message });
        }

        var blogListData = _blogService.GetBlogList(blogListAdminFilter, uid);
        var blogReturn = new BlogReturn<BLogListData>(blogListData);
        return Ok(blogReturn);
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
      }
    }

    [HttpPut("updateStatus/{id}")]
    public IActionResult UpdateStatus([FromRoute] int id, [FromQuery] BlogStatus status)
    {
      try
      {
        int uid;
        try
        {
          uid = _accountService.GetUserIdFromToken();
        }
        catch (Exception ex)
        {
          return Unauthorized(new { message = ex.Message });
        }

        if (!_accountService.IsAdmin())
        {
          return Unauthorized(new { message = "You are not Admin" });
        }

        _blogService.UpdateStatus(id, status);
        return StatusCode(200, "Success");
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
      }
    }
  }
}
