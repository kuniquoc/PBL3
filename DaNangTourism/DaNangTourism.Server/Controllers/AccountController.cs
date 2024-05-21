using DaNangTourism.Server.DAL;
using DaNangTourism.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace DaNangTourism.Server.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController(IConfiguration configuration) : Controller
    {
        readonly AccountDAO _accountDAO = AccountDAO.Instance;
        private readonly IConfiguration _configuration = configuration;


        #region HTTP Methods
        [HttpGet("verify")]
        public ActionResult VerifyToken(string token)
        {
            var secret = _configuration["Secret"];
            var key = new SymmetricSecurityKey(key: secret != null ? System.Text.Encoding.UTF8.GetBytes(secret) : throw new ArgumentNullException(nameof(secret)));

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var claims = jwtToken.Claims.ToDictionary(claim => claim.Type, claim => claim.Value);

                return Ok(claims);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }


        //FOR USERS
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
            var accountDB = GetAccountByEmail(account.Email);
            if (accountDB == null)
            {
                return NotFound();
            }
            if (!VerifyPasswordHash(account.Password, accountDB.PasswordHash, accountDB.PasswordSalt))
            {
                return Unauthorized();
            }

            _ = GenerateJwtToken(accountDB);

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
                status = 200,
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
            if (GetAccountByEmail(account.Email) != null)
            {
                return Conflict();
            }

            CreatePasswordHash(account.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var newAccount = new Account
            {
                Email = account.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Permission = Permission.user
            };

            _accountDAO.AddAccount(newAccount);

            var response = new
            {
                status = 201,
                message = "Register successful"
            };

            return CreatedAtAction(nameof(Register), response); ;
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
        public ActionResult Authenticated(string token)
        {
            var secret = _configuration["Secret"];
            var key = new SymmetricSecurityKey(secret != null ? System.Text.Encoding.UTF8.GetBytes(secret) : throw new ArgumentNullException(nameof(secret)));

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var claims = jwtToken.Claims.ToDictionary(claim => claim.Type, claim => claim.Value);

                // Get account by id
                var account = GetAccountById(int.Parse(claims["id"]));
                if (account == null)
                {
                    return NotFound();
                }

                var userData = new
                {
                    id = account.Id,
                    name = account.Name,
                    avatar = account.Avatar,
                    email = account.Email,
                    role = account.Permission.ToString()
                };

                var response = new
                {
                    status = 200,
                    message = "Authenticated",
                    data = userData
                };

                return Ok(response);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /* Get user data by token
        request header:
        token: {token}
        response body:
        {
        	"status": 200,
        	"message": "Get user data successful",
        	"data": {
        		"id": 10000001,
        		"name": "John Doe",
        		"avatar": "https://example.com/avatar.jpg",
        		"email": "example@email.com",
        		"role": "user",
        		"dateOfBirth": "2024-05-19T03:31:09.229Z" // Datetime string in ISO 8601 format
        	}
        }
        */
        [HttpGet("getaccountbytoken")]
        public ActionResult GetAccountByToken(string token)
        {
            var secret = _configuration["Secret"];
            var key = new SymmetricSecurityKey(secret != null ? System.Text.Encoding.UTF8.GetBytes(secret) : throw new ArgumentNullException(nameof(secret)));

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var claims = jwtToken.Claims.ToDictionary(claim => claim.Type, claim => claim.Value);

                // Get account by id
                var account = GetAccountById(int.Parse(claims["id"]));
                if (account == null)
                {
                    return NotFound();
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
                    status = 200,
                    message = "Get user data successful",
                    data = userData
                };

                return Ok(response);

            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        /* Update user data
        request header:
        {
	        "name": "John Doe",
	        "avatar": "https://example.com/avatar.jpg",
	        "dateOfBirth": "2024-05-19T03:31:09.229Z"
        }
        response body:
        {
            "status": 200,
            "message": "Update user data successful"
        }
        */
        [HttpPut("updateaccount")]
        public ActionResult UpdateAccount([FromBody] AccountUpdateModel account, string token)
        {
            var secret = _configuration["Secret"];
            var key = new SymmetricSecurityKey(secret != null ? System.Text.Encoding.UTF8.GetBytes(secret) : throw new ArgumentNullException(nameof(secret)));

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var claims = jwtToken.Claims.ToDictionary(claim => claim.Type, claim => claim.Value);

                // Get account by id
                var accountDB = GetAccountById(int.Parse(claims["id"]));
                if (accountDB == null)
                {
                    return NotFound();
                }

                accountDB.Name = account.Name;
                accountDB.Avatar = account.Avatar;
                accountDB.Birthday = account.Birthday;

                _accountDAO.UpdateAccount(accountDB);

                var response = new
                {
                    status = 200,
                    message = "Update user data successful"
                };

                return Ok(response);

            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
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
        [HttpPut("changepassword")]
        public ActionResult ChangePassword([FromBody] AccountChangePasswordModel account, string token)
        {
            var secret = _configuration["Secret"];
            var key = new SymmetricSecurityKey(secret != null ? System.Text.Encoding.UTF8.GetBytes(secret) : throw new ArgumentNullException(nameof(secret)));

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var claims = jwtToken.Claims.ToDictionary(claim => claim.Type, claim => claim.Value);

                // Get account by id
                var accountDB = GetAccountById(int.Parse(claims["id"]));
                if (accountDB == null)
                {
                    return NotFound();
                }

                if (!VerifyPasswordHash(account.OldPassword, accountDB.PasswordHash, accountDB.PasswordSalt))
                {
                    return Unauthorized();
                }

                CreatePasswordHash(account.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);

                accountDB.PasswordHash = passwordHash;
                accountDB.PasswordSalt = passwordSalt;

                _accountDAO.UpdateAccount(accountDB);

                var response = new
                {
                    status = 200,
                    message = "Change password successful"
                };

                return Ok(response);

            }
            catch (Exception ex)
            {
                return Unauthorized(ex);
            }
        }

        //FOR ADMIN
        /* User list with token for admin to manage
        request header:
        {
            page: 1,
            limit: 15,
            search: "John Doe"
            role: "user"
            sortBy: "name"
            sortType: "asc"
        }
        response body:
        {
	        "status": 200,
	        "message": "Success",
	        "data": {
	        	"total": 100,
	        	"page": 1,
	        	"limit": 12,
	        	"items": [
	        		{
	        			"id": 10000001,
	        			"name": "John Doe",
	        			"email": "example@email.com",
	        			"createdAt": "2024-05-19T03:31:09.229Z", // Datetime string in ISO 8601 format
	        			"role": "user"
	        		},
	        		{
	        			"id": 10000002,
	        			"name": "Jane Doe",
	        			"email": "example@email.com",
	        			"createdAt": "2024-05-19T03:31:09.229Z", // Datetime string in ISO 8601 format
	        			"role": "admin"
	        		}
	        	]
	        }
        }
        */
        [HttpGet("userlist")]
        public ActionResult UserList(string token, int page, int limit, string search, string role, string sortBy, string sortType)
        {
            var secret = _configuration["Secret"];
            var key = new SymmetricSecurityKey(secret != null ? System.Text.Encoding.UTF8.GetBytes(secret) : throw new ArgumentNullException(nameof(secret)));

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var claims = jwtToken.Claims.ToDictionary(claim => claim.Type, claim => claim.Value);

                // Kiểm tra quyền admin
                var accountDB = GetAccountById(int.Parse(claims["id"]));
                if (accountDB == null || accountDB.Permission != Permission.admin)
                {
                    return Unauthorized();
                }

                //Lấy toàn bộ tài khoản
                Dictionary<int, Account> result = _accountDAO.GetAllAccounts();
                //Tìm kiếm tài khoản theo tên
                if (search != null)
                {
                    result = SearchAccountByName(result, search);
                }
                //Lọc tài khoản theo role
                if (role != null)
                {
                    Permission roleEnum = (Permission)Enum.Parse(typeof(Permission), role);
                    result = result.Where(x => x.Value.Permission == roleEnum).ToDictionary(x => x.Key, x => x.Value);
                }
                //Lấy số lượng tài khoản
                int total = result.Count;
                //Sắp xếp tài khoản
                if (sortBy != null)
                {
                    result = SortAccount(result, sortBy, sortType);
                }
                //Lấy tài khoản theo trang
                result = GetAccountsByPage(result, page, limit);

                var items = new ArrayList();
                foreach (var account in result)
                {
                    items.Add(new
                    {
                        id = account.Value.Id,
                        name = account.Value.Name,
                        email = account.Value.Email,
                        createdAt = account.Value.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                        role = account.Value.Permission.ToString()
                    });
                }

                var response = new
                {
                    status = 200,
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
            catch (Exception ex)
            {
                return Unauthorized(ex);
            }
        }

        /* Update user role
        request header:
        {
        	"id": 10000001,
        	"role": "admin"
        }
        response body:
        {
        	"status": 200,
        	"message": "Update user role successful"
        }
        */
        [HttpPut("updateuserrole")]
        public ActionResult UpdateUserRole(int id, string token)
        {
            var secret = _configuration["Secret"];
            var key = new SymmetricSecurityKey(secret != null ? System.Text.Encoding.UTF8.GetBytes(secret) : throw new ArgumentNullException(nameof(secret)));

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var claims = jwtToken.Claims.ToDictionary(claim => claim.Type, claim => claim.Value);

                // Kiểm tra quyền admin
                var accountDB = GetAccountById(int.Parse(claims["id"]));
                if (accountDB == null || accountDB.Permission != Permission.admin)
                {
                    return Unauthorized();
                }

                var account = GetAccountById(id);
                if (account == null)
                {
                    return NotFound();
                }

                account.Permission = Permission.admin;

                _accountDAO.UpdateAccount(account);

                var response = new
                {
                    status = 200,
                    message = "Update user role successful"
                };

                return Ok(response);

            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        /* Delete user
        request header:
        {
        	"id": 10000001
        }
        response body:
        {
        	"status": 200,
        	"message": "Delete user successful"
        }
        */
        [HttpDelete("deleteuser")]
        public ActionResult DeleteUser(int id, string token)
        {
            var secret = _configuration["Secret"];
            var key = new SymmetricSecurityKey(secret != null ? System.Text.Encoding.UTF8.GetBytes(secret) : throw new ArgumentNullException(nameof(secret)));

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var claims = jwtToken.Claims.ToDictionary(claim => claim.Type, claim => claim.Value);

                // Kiểm tra quyền admin
                var accountDB = GetAccountById(int.Parse(claims["id"]));
                if (accountDB == null || accountDB.Permission != Permission.admin)
                {
                    return Unauthorized();
                }

                var account = GetAccountById(id);
                if (account == null)
                {
                    return NotFound();
                }

                _accountDAO.DeleteAccount(id);

                var response = new
                {
                    status = 200,
                    message = "Delete user successful"
                };

                return Ok(response);

            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        #endregion

        #region 
        // Lấy tài khoản theo id
        private Account? GetAccountById(int id)
        {
            Account? result = null;
            Dictionary<int, Account> accounts = _accountDAO.GetAllAccounts();
            if (accounts.TryGetValue(id, out Account? value))
            {
                result = value;
            }
            return result;
        }

        // Lấy tài khoản theo email
        private Account? GetAccountByEmail(string email)
        {
            Account? result = null;
            Dictionary<int, Account> accounts = _accountDAO.GetAllAccounts();
            foreach (var account in accounts)
            {
                if (account.Value.Email == email)
                {
                    result = account.Value;
                    break;
                }
            }
            return result;
        }

        // Tạo password hash và salt
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        // Tìm kiếm tài khoản theo tên
        private static Dictionary<int, Account> SearchAccountByName(Dictionary<int, Account> accounts, string name)
        {
            Dictionary<int, Account> result = [];
            foreach (var account in accounts)
            {
                if (account.Value.Name.Contains(name))
                {
                    result.Add(account.Key, account.Value);
                }
            }
            return result;
        }

        // Sắp xếp tài khoản theo tên hoặc ngày tạo
        private static Dictionary<int, Account> SortAccount(Dictionary<int, Account> accounts, string sortBy, string sortType)
        {
            Dictionary<int, Account> result = [];
            if (sortBy == "name")
            {
                if (sortType == "asc")
                {
                    foreach (var account in accounts.OrderBy(x => x.Value.Name))
                    {
                        result.Add(account.Key, account.Value);
                    }
                }
                else
                {
                    foreach (var account in accounts.OrderByDescending(x => x.Value.Name))
                    {
                        result.Add(account.Key, account.Value);
                    }
                }
            }
            else if (sortBy == "createdAt")
            {
                if (sortType == "asc")
                {
                    foreach (var account in accounts.OrderBy(x => x.Value.CreatedAt))
                    {
                        result.Add(account.Key, account.Value);
                    }
                }
                else
                {
                    foreach (var account in accounts.OrderByDescending(x => x.Value.CreatedAt))
                    {
                        result.Add(account.Key, account.Value);
                    }
                }
            }
            return result;
        }

        //Hiển thị danh sách tài khoản theo trang
        private static Dictionary<int, Account> GetAccountsByPage(Dictionary<int, Account> accounts, int page, int limit)
        {
            Dictionary<int, Account> result = [];
            int start = (page - 1) * limit;
            int end = start + limit;
            int index = 0;
            foreach (var account in accounts)
            {
                if (index >= start && index < end)
                {
                    result.Add(account.Key, account.Value);
                }
                index++;
            }
            return result;
        }

        // Xác thực password
        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }

        // Tạo token
        private string GenerateJwtToken(Account account)
        {
            var claims = new List<Claim>
            {
                new("id", account.Id.ToString()),
                new("role", account.Permission.ToString()),
                new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var secret = _configuration["Secret"];
            var key = new SymmetricSecurityKey(secret != null ? Encoding.UTF8.GetBytes(secret) : throw new ArgumentNullException(nameof(secret)));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        #endregion
    }
}
