using DaNangTourism.Server.DAL;
using DaNangTourism.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;


namespace DaNangTourism.Server.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController : Controller
    {
        AccountDAO accountDAO = AccountDAO.Instance;

        [HttpGet("get/all")]
        public IActionResult GetAllAccounts()
        {
            Dictionary<int, Account> accounts = accountDAO.GetAllAccounts();
            byte[] a = new byte[] {0,1,1,0 };
            string a1 = Encoding.UTF8.GetString(a);
            Console.WriteLine(a);
            byte[] a2 = Encoding.UTF8.GetBytes(a1);
            Console.WriteLine(a1);
            Console.WriteLine(a2);

            if (accounts.Count == 0)
            { 
                return NotFound();
            }
            else return Ok(accounts); 
        }

        [HttpGet("get/{id}")]
        public IActionResult GetAccountById(int id)
        {
            Account account = accountDAO.GetAccountById(id);
            if (account == null)
            {
                return NotFound();
            }
            else return Ok(account);
        }

        [HttpPost("add")]
        public IActionResult AddAccount([FromBody] AccountRegister account)
        {
            //Tạo passwordhash và passwordsalt
            CreatePasswordHash(account.Password, out byte[] passwordHash, out byte[] passwordSalt);
            //string PasswordHash = Encoding.UTF8.GetString(passwordHash);
            //string PasswordSalt = Encoding.UTF8.GetString(passwordSalt);

            int check = accountDAO.AddAccount(account, passwordHash, passwordSalt);
            if (check > 0)
            {
                return Ok();
            }
            else return BadRequest();
        }

        [HttpPut("update")]
        public IActionResult UpdateAccountInformation([FromBody] Account account)
        {
            int check = accountDAO.UpdateAccountÌnformation(account);
            if (check > 0)
            {
                return NoContent();
            }
            else return BadRequest();
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteAccount(int id)
        {
            int check = accountDAO.DeleteAccount(id);
            if (check > 0)
            {
                return NoContent();
            }
            else return BadRequest();
        }

        //Đăng ký tài khoản
        [HttpPost("register")]
        public ActionResult<Account> Register([FromBody] AccountRegister newAccount)
        {
            //Kiểm tra tài khoản tồn tại
            if (accountDAO.CheckAccountExist(newAccount.Username))
            {
                return BadRequest();
            }
            //Tạo passwordhash và passwordsalt
            CreatePasswordHash(newAccount.Password, out byte[] passwordHash, out byte[] passwordSalt);
            //string PasswordHash = Encoding.ASCII.GetString(passwordHash);
            //string PasswordSalt = Encoding.ASCII.GetString(passwordSalt);
            //Thêm tài khoản
            int check = accountDAO.AddAccount(newAccount, passwordHash, passwordSalt);
            if (check > 0)
            {
                return Ok();
            }
            else return BadRequest();
        }

        //Tạo passwordhash và passwordsalt sử dụng HMACSHA512
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        //Kiểm tra đăng nhập
        [HttpPost("login")]
        public ActionResult<Account> Login([FromBody] AccountLogin accountLogin)
        {
            //Kiểm tra tài khoản tồn tại
            if (!accountDAO.CheckAccountExist(accountLogin.Username))
            {
                return NotFound();
            }
            //Lấy thông tin tài khoản
            var accountDB = accountDAO.GetAccountByUsername(accountLogin.Username);
        //    //Kiểm tra password
        //    byte[] pH = Convert.FromBase64String(accountDB.PasswordHash);
        //    byte[] pS = Convert.FromBase64String(accountDB.PasswordSalt);
            if (!VerifyPasswordHash(accountLogin.Password, accountDB.PasswordHash, accountDB.PasswordSalt))
            {
                return Unauthorized();
            }
            //Tạo token jwt
            string token = GenerateJwtToken(accountDB);
            return Ok(new { token });
        }

        //Kiểm tra password có đúng không
        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        //Tạo token jwt cho tài khoản
        private string GenerateJwtToken(Account account)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Name, account.Username),
                new Claim(ClaimTypes.Role, account.Permission.ToString())
            };
            //Tạo token
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes("Secret"));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha512Signature);
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            JwtSecurityTokenHandler tokenHandler = new();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        //Verify token
        [HttpPost("verify")]
        public ActionResult VerifyToken(string token)
        {
            //Kiểm tra token
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes("super secret key"));
            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = false,
                ValidateAudience = false
            };
            JwtSecurityTokenHandler tokenHandler = new();
            try
            {
                tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
            }
            catch
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
