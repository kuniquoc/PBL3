using DaNangTourism.Server.Models;
using DaNangTourism.Server.Models.SecurityModels;
using DaNangTourism.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace DaNangTourism.Server.Controllers
{
  [ApiController]
  [Route("auth")]
  public class AuthController : Controller
  {
    private readonly IAccountService _accountService;
    private readonly IEmailService _emailService;

    public AuthController(IAccountService accountService, IEmailService emailService)
    {
      _accountService = accountService;
      _emailService = emailService;
    }

    #region HTTP Methods

    [HttpPost("login")]
    public ActionResult Login([FromBody] AccountLoginModel account)
    {
      var accountDB = _accountService.GetAccountByEmail(account.Email);
      if (accountDB == null)
      {
        return NotFound(new { message = "Email not found" });
      }
      if (!VerifyPasswordHash(account.Password, accountDB.PasswordHash, accountDB.PasswordSalt))
      {
        return Unauthorized(new { message = "Incorrect password" });
      }

      _ = _accountService.GenerateJwtToken(accountDB, Response, account.RememberMe);

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


    [HttpPost("register")]
    public ActionResult Register([FromBody] AccountRegisterModel account)
    {
      try
      {
        if (_accountService.GetAccountByEmail(account.Email) != null)
        {
          return Conflict(new { message = "Email already exists" });
        }

        try
        {
          bool correctCode = _accountService.CheckConfirmCode(account.Email, account.Code);
          if (!correctCode)
          {
            return BadRequest(new { message = "Incorrect confirmation code" });
          }
        }
        catch (UnauthorizedAccessException uae)
        {
          return BadRequest(new { message = uae.Message });
        }

        CreatePasswordHash(account.Password, out byte[] passwordHash, out byte[] passwordSalt);

        // Tạo tài khoản mới
        var newAccount = new Account
        {
          Email = account.Email,
          PasswordHash = passwordHash,
          PasswordSalt = passwordSalt,
          Permission = Permission.user
        };

        // Thêm tài khoản mới vào cơ sở dữ liệu
        _accountService.AddAccount(newAccount);

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
        Console.WriteLine(ex.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
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

    [HttpGet("authenticated")]
    public ActionResult Authenticated()
    {
      try
      {
        // Lấy thông tin xác thực từ cookie
        var claims = _accountService.GetClaimsByCookie();

        // Kiểm tra xem có tồn tại id trong cookie không
        if (!claims.ContainsKey("id"))
        {
          return Unauthorized(new { message = "Invalid authentication data" });
        }

        // Lấy tài khoản dựa trên id từ cookie
        var accountId = int.Parse(claims["id"]);
        var account = _accountService.GetAccountById(accountId);

        // Kiểm tra xem tài khoản có tồn tại không
        if (account == null)
        {
          return NotFound(new { message = "Account not found" });
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
      catch (UnauthorizedAccessException uae)
      {
        return Unauthorized(new { message = uae.Message });
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
      }
    }

    [HttpPut("update/password")]
    public ActionResult ChangePassword([FromBody] AccountChangePasswordModel account)
    {
      try
      {
        // Lấy thông tin xác thực từ cookie
        var claims = _accountService.GetClaimsByCookie();

        // Kiểm tra xem có tồn tại id trong cookie không
        if (!claims.ContainsKey("id"))
        {
          return Unauthorized(new { message = "Invalid authentication data" });
        }

        // Lấy tài khoản dựa trên id từ cookie
        var accountId = int.Parse(claims["id"]);
        var accountDB = _accountService.GetAccountById(accountId);

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
        _accountService.UpdateAccount(accountDB);

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
        Console.WriteLine(ex.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
      }
    }

    [HttpPost("sendCode")]
    public ActionResult SendConfirmCode([FromBody] InputCreateCode createCode)
    {
      try
      {
        string code = _accountService.CreateConfirmCode(createCode.Email, 5);
        _emailService.SendEmailAsync(createCode.Email, "Your account confirmation code", "Your confirmation code is: <b>" + code + "</b>. This code will expire in 5 minutes.");
        // Trả về thông điệp thành công
        var response = new
        {
          status = 200,
          message = "Your confirmation code has been sent to your email"
        };

        return Ok(response);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
      }
    }

    [HttpPost("resetPassword")]
    public ActionResult ResetPassword([FromBody] AccountResetPasswordModel account)
    {
      try
      {
        var accountDB = _accountService.GetAccountByEmail(account.Email);
        if (accountDB == null)
        {
          return NotFound(new { message = "Email not found" });
        }

        try
        {
          bool correctCode = _accountService.CheckConfirmCode(account.Email, account.Code);
          if (!correctCode)
          {
            return BadRequest(new { message = "Incorrect confirmation code" });
          }
        }
        catch (UnauthorizedAccessException uae)
        {
          return BadRequest(new { message = uae.Message });
        }

        string newPassword = GenerateRandomPassword();
        CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);

        accountDB.PasswordHash = passwordHash;
        accountDB.PasswordSalt = passwordSalt;
        _accountService.UpdateAccount(accountDB);

        _emailService.SendEmailAsync(account.Email, "You have reset your password", "Your new password is: <b>" + newPassword + "</b>. Please change it after login.");

        var response = new
        {
          message = "Reset password successful"
        };

        return Ok(response);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return StatusCode(500, new { message = "Something went wrong, please try again later." });
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

    // Tạo mật khẩu ngẫu nhiên
    private static string GenerateRandomPassword()
    {
      const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
      var random = new Random();
      return new string(Enumerable.Repeat(chars, 12)
        .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    #endregion

  }
}
