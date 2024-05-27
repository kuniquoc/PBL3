using DaNangTourism.Server.DAL;
using DaNangTourism.Server.Models;
using DaNangTourism.Server.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DaNangTourism.Server.Controllers
{
  [ApiController]
  [Route("auth")]
  public class AuthController(IConfiguration configuration) : Controller
  {
    private readonly AccountDAO _accountDAO = AccountDAO.Instance;
    private readonly IConfiguration _configuration = configuration;
    private readonly AccountBLL _accountBLL = AccountBLL.Instance(configuration);

    #region HTTP Methods

    /* Login
    request body:
    {
      "email": "example@email.com",
      "password": "123456"
    }
    response body:
    {
      "status": 200,
      "message": "Login successful",
      "data": {
        "id": 10000001,
        "name": "John Doe",
        "avatar": "https://example.com/avatar.jpg",
        "email": "example@email.com",
        "role": "user"
          }
    }
    */

    [HttpPost("login")]
    public ActionResult Login([FromBody] AccountLoginModel account)
    {
      var accountDB = _accountDAO.GetAccountByEmail(account.Email);
      if (accountDB == null)
      {
        return NotFound(new { message = "Email not found" });
      }
      if (!VerifyPasswordHash(account.Password, accountDB.PasswordHash, accountDB.PasswordSalt))
      {
        return Unauthorized(new { message = "Incorrect password" });
      }

      _ = _accountBLL.GenerateJwtToken(accountDB, Response, account.RememberMe);

      var userData = new
      {
        id = accountDB.Id,
        name = accountDB.Name,
        avatar = accountDB.Avatar,
        email = accountDB.Email,
        role = accountDB.Permission.ToString()
      };

      var response = new
      {
        message = "Login successful",
        data = userData,
      };

      return Ok(response);
    }


    /* Register
    request body:
    {
        "name": "John Doe",
        "email": "example@email.com",
        "password": "123456"
    }
    response body:
    {
        "status": 201,
        "message": "Register successful"
    }
    */

    [HttpPost("register")]
    public ActionResult Register([FromBody] AccountRegisterModel account)
    {
      try
      {
        // Kiểm tra xem email đã tồn tại hay chưa
        if (_accountDAO.GetAccountByEmail(account.Email) != null)
        {
          return Conflict("Email already exists");
        }

        // Tạo hash và salt cho mật khẩu
        CreatePasswordHash(account.Password, out byte[] passwordHash, out byte[] passwordSalt);

        // Tạo tài khoản mới
        var newAccount = new Account
        {
          Name = account.Name,
          Email = account.Email,
          PasswordHash = passwordHash,
          PasswordSalt = passwordSalt,
          Permission = Permission.user
        };

        // Thêm tài khoản mới vào cơ sở dữ liệu
        _accountDAO.AddAccount(newAccount);

        // Tạo thông điệp phản hồi
        var response = new
        {
          message = "Register successful"
        };

        // Trả về mã 201 Created và thông điệp phản hồi
        return CreatedAtAction(nameof(Register), response);
      }
      catch (Exception ex)
      {
        // Xử lý các ngoại lệ và trả về mã lỗi 500 Internal Server Error cùng với thông điệp lỗi
        return StatusCode(500, $"Internal server error: {ex.Message}");
      }
    }

    // Logout
    [HttpGet("logout")]
    public ActionResult Logout()
    {
      Response.Cookies.Delete("jwtToken");
      var response = new
      {
        message = "Logout successful"
      };
      return Ok(response);
    }


    /* Authenticed user by token
    request header:
    token: {token}
    response body:
    {
        "status": 200,
        "message": "Authenticated",
        "data": {
        "id": 10000001,
        "name": "John Doe",
        "avatar": "https://example.com/avatar.jpg",
        "email": "example@email.com",
        "role": "user"
        }
    }
    */
    [HttpGet("authenticated")]
    public ActionResult Authenticated()
    {
      try
      {
        // Lấy thông tin xác thực từ cookie
        var claims = _accountBLL.GetClaimsByCookie(HttpContext);

        // Kiểm tra xem có tồn tại id trong cookie không
        if (!claims.ContainsKey("id"))
        {
          return Unauthorized("Invalid authentication data");
        }

        // Lấy tài khoản dựa trên id từ cookie
        var accountId = int.Parse(claims["id"]);
        var account = _accountDAO.GetAccountById(accountId);

        // Kiểm tra xem tài khoản có tồn tại không
        if (account == null)
        {
          return NotFound("Account not found");
        }

        // Tạo đối tượng userData từ thông tin tài khoản
        var userData = new
        {
          id = account.Id,
          name = account.Name,
          avatar = account.Avatar,
          email = account.Email,
          role = account.Permission.ToString()
        };

        // Tạo thông điệp phản hồi thành công
        var response = new
        {
          message = "Authenticated",
          data = userData
        };

        // Trả về mã 200 OK cùng với thông điệp phản hồi
        return Ok(response);
      }
      catch (Exception ex)
      {
        // Xử lý các ngoại lệ và trả về mã lỗi 401 Unauthorized cùng với thông điệp lỗi
        return Unauthorized($"Authentication failed: {ex.Message}");
      }
    }


    /* Change password
    request header:
    {
        "oldPassword": "123456",
        "newPassword": "654321"
    }
    response body:
    {
        "status": 200,
        "message": "Change password successful"
    }
    */
    [HttpPut("update/password")]
    public ActionResult ChangePassword([FromBody] AccountChangePasswordModel account)
    {
      try
      {
        // Lấy thông tin xác thực từ cookie
        var claims = _accountBLL.GetClaimsByCookie(HttpContext);

        // Kiểm tra xem có tồn tại id trong cookie không
        if (!claims.ContainsKey("id"))
        {
          return Unauthorized(new { message = "Invalid authentication data" });
        }

        // Lấy tài khoản dựa trên id từ cookie
        var accountId = int.Parse(claims["id"]);
        var accountDB = _accountDAO.GetAccountById(accountId);

        // Kiểm tra xem tài khoản có tồn tại không
        if (accountDB == null)
        {
          return NotFound(new { message = "Account not found" });
        }

        // Xác thực mật khẩu cũ
        if (!VerifyPasswordHash(account.OldPassword, accountDB.PasswordHash, accountDB.PasswordSalt))
        {
          return Unauthorized(new { message = "Incorrect old password" });
        }

        // Tạo hash và salt mới cho mật khẩu mới
        CreatePasswordHash(account.NewPassword, out byte[] newPasswordHash, out byte[] newPasswordSalt);

        // Cập nhật mật khẩu trong cơ sở dữ liệu
        accountDB.PasswordHash = newPasswordHash;
        accountDB.PasswordSalt = newPasswordSalt;
        _accountDAO.UpdateAccount(accountDB);

        // Trả về thông điệp thành công
        var response = new
        {
          status = 200,
          message = "Change password successful"
        };

        return Ok(response);
      }
      catch (Exception ex)
      {
        // Trả về mã lỗi 401 Unauthorized cùng với thông điệp lỗi
        return Unauthorized(new { message = $"Change password failed: {ex.Message}" });
      }
    }

    #endregion

    #region Private Methods 

    // Tạo password hash và salt
    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
      using var hmac = new HMACSHA512();
      passwordSalt = hmac.Key;
      passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }

    // Xác thực password
    private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
      using var hmac = new HMACSHA512(passwordSalt);
      var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
      return computedHash.SequenceEqual(passwordHash);
    }

    #endregion

  }
}
