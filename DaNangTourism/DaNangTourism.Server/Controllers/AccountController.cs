using DaNangTourism.Server.Models;
using Microsoft.AspNetCore.Mvc;
using DaNangTourism.Server.Services;


namespace DaNangTourism.Server.Controllers
{
  [ApiController]
  [Route("account")]
  public class AccountController : Controller
  {
    private readonly IAccountService _accountService;
    public AccountController(IAccountService accountService)
    {
      _accountService = accountService;
    }


    #region HTTP Methods

    [HttpGet("get")]
    public ActionResult GetAccountByCookie()
    {
      try
      {
        // Get account by id
        var account = _accountService.GetAccountById(_accountService.GetUserIdFromToken());
        if (account == null)
        {
          return NotFound(new { message = "User not found" });
        }

        var userData = new
        {
          id = account.Id,
          name = account.Name,
          avatar = account.Avatar,
          email = account.Email,
          role = account.Permission.ToString(),
          dateOfBirth = account.Birthday.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
        };

        var response = new
        {
          message = "Get user data successful",
          data = userData
        };

        return Ok(response);
      }
      catch (ArgumentNullException ae)
      {
        return Unauthorized(new { message = ae.Message });
      }
      catch (UnauthorizedAccessException uae)
      {
        return Unauthorized(new { message = uae.Message });
      }
      catch (Exception e)
      {
        return StatusCode(500, new { message = e.Message });
      }

    }

    [HttpPut("update")]
    public ActionResult UpdateAccount([FromBody] AccountUpdateModel account)
    {
            Console.WriteLine(account.Name + " " + account.Avatar + " " + account.Birthday);
      try
      {
        // Get account by id
        var accountDB = _accountService.GetAccountById(_accountService.GetUserIdFromToken());
        if (accountDB == null)
        {
          return NotFound(new { message = "User not found" });
        }

        accountDB.Name = account.Name;
        accountDB.Avatar = account.Avatar;
        accountDB.Birthday = account.Birthday;

        _accountService.UpdateAccount(accountDB);

        var response = new
        {
          message = "Update user data successful"
        };

        return Ok(response);

      }
      catch (ArgumentNullException ae)
      {
        return Unauthorized(new { message = ae.Message });
      }
      catch (UnauthorizedAccessException uae)
      {
        return Unauthorized(new { message = uae.Message });
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
      }
    }

    [HttpGet("get/list")]
    public ActionResult UserList(string? search, int page = 1, int limit = 15, string role = "user", string sortBy = "created_at", string sortType = "desc")
    {
      // Validate page and limit
      if (page <= 0)
      {
        return BadRequest("Page number must be greater than 0");
      }
      if (limit <= 0)
      {
        return BadRequest("Limit must be greater than 0");
      }

      try
      {

        // Check if user is admin
        var accountDB = _accountService.GetAccountById(_accountService.GetUserIdFromToken());
        if (accountDB == null || accountDB.Permission != Permission.admin)
        {
          return Unauthorized("Only admin can access this endpoint");
        }

        // Search for accounts
        var result = _accountService.SearchAccount(search, page, limit, role, sortBy, sortType);
        int total = _accountService.GetTotalAccounts(search, role);

        // Convert accounts to anonymous objects
        var items = result.Select(account =>
            new
            {
              id = account.Value.Id,
              name = account.Value.Name,
              email = account.Value.Email,
              createdAt = account.Value.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
              role = account.Value.Permission.ToString()
            }
        ).ToList();

        // Create response object
        var response = new
        {
          message = "Success",
          data = new
          {
            total,
            page,
            limit,
            items
          }
        };

        return Ok(response);
      }
      catch (ArgumentNullException ae)
      {
        return Unauthorized(new { message = ae.Message });
      }
      catch (UnauthorizedAccessException uae)
      {
        return Unauthorized(new { message = uae.Message });
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
      }
    }

    [HttpPut("update/role/{id}")]
    public ActionResult UpdateUserRole(int id)
    {
      try
      {
        // Kiểm tra quyền admin
        var accountDB = _accountService.GetAccountById(_accountService.GetUserIdFromToken());
        if (accountDB == null || accountDB.Permission != Permission.admin)
        {
          return Unauthorized("Only admin can update user roles");
        }

        // Lấy tài khoản cần cập nhật
        var account = _accountService.GetAccountById(id);
        if (account == null)
        {
          return NotFound("User not found");
        }

        // Cập nhật quyền cho tài khoản
        if (account.Permission == Permission.admin)
        {
          account.Permission = Permission.user;
        }
        else
        {
          account.Permission = Permission.admin;
        }
        _accountService.UpdateAccount(account);

        var response = new
        {
          message = "User role updated to \"" + account.Permission.ToString() + "\""
        };

        return Ok(response);
      }
      catch (ArgumentNullException ae)
      {
        return Unauthorized(new { message = ae.Message });
      }
      catch (UnauthorizedAccessException uae)
      {
        return Unauthorized(new { message = uae.Message });
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
      }
    }

    [HttpDelete("delete/{id}")]
    public ActionResult DeleteUser(int id)
    {
      try
      {
        // Kiểm tra quyền admin
        var accountDB = _accountService.GetAccountById(_accountService.GetUserIdFromToken());
        if (accountDB == null || accountDB.Permission != Permission.admin)
        {
          return Unauthorized("Only admin can delete users");
        }

        // Lấy tài khoản cần xóa
        var account = _accountService.GetAccountById(id);
        if (account == null)
        {
          return NotFound("User not found");
        }

        // Xóa tài khoản
        _accountService.DeleteAccount(id);

        var response = new
        {
          message = "Delete user successful"
        };

        return Ok(response);
      }
      catch (ArgumentNullException ae)
      {
        return Unauthorized(new { message = ae.Message });
      }
      catch (UnauthorizedAccessException uae)
      {
        return Unauthorized(new { message = uae.Message });
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
      }
    }

    #endregion

  }
}
