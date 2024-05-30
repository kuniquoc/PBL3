using DaNangTourism.Server.DAL;
using DaNangTourism.Server.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace DaNangTourism.Server.Services
{
    public interface IAccountService
    {
        Dictionary<string, string> GetClaimsByCookie();
        string GenerateJwtToken(Account account, HttpResponse response, bool rememberMe = false);
        void SetJwtTokenCookie(string token, HttpResponse response, bool rememberMe = false);
        string? GetTokenFromCookie(HttpContext httpContext);
        int GetUserIdFromToken();
        bool IsAdmin();
        string GetUserName();   
        Account? GetAccountById(int id);
        void UpdateAccount(Account account);
        Dictionary<int, Account> SearchAccount(string? search, int page, int limit, string role, string sortBy, string sortType);
        int GetTotalAccounts(string? search, string role = "user");
        void DeleteAccount(int id);
        Account? GetAccountByEmail(string email);
        void AddAccount(Account account);

    }
    public class AccountService: IAccountService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;

        public AccountService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IAccountRepository accountRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _accountRepository = accountRepository;

        }

        public string GenerateJwtToken(Account account, HttpResponse response, bool rememberMe = false)
        {
            var claims = new List<Claim>
                {
                    new("id", account.Id.ToString()),
                    new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
                };

            var secret = _configuration["Secret"];
            var key = new SymmetricSecurityKey(secret != null ? Encoding.UTF8.GetBytes(secret) : throw new ArgumentNullException(nameof(secret)));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: rememberMe ? DateTime.UtcNow.AddDays(1) : DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            // Tạo và thiết lập cookie
            SetJwtTokenCookie(jwt, response, rememberMe);

            return jwt;
        }

        public void SetJwtTokenCookie(string token, HttpResponse response, bool rememberMe = false)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Expires = rememberMe ? DateTime.UtcNow.AddDays(1) : null,
            };
            response.Cookies.Append("jwtToken", token, cookieOptions);
        }

        public string? GetTokenFromCookie(HttpContext httpContext)
        {
            if (httpContext.Request.Cookies.ContainsKey("jwtToken"))
            {
                return httpContext.Request.Cookies["jwtToken"];
            }
            else
            {
                throw new UnauthorizedAccessException("Token not found in cookies");
            }
        }

        //Lấy claims theo cookie
        public Dictionary<string, string> GetClaimsByCookie()
        {
            HttpContext? httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                throw new UnauthorizedAccessException("HttpContext is missing");
            }
            // Lấy token từ cookie
            var token = GetTokenFromCookie(httpContext);

            // Từ token lấy ra các claims
            var secret = _configuration["Secret"];
            var key = new SymmetricSecurityKey(secret != null ? System.Text.Encoding.UTF8.GetBytes(secret) : throw new ArgumentNullException(nameof(secret)));

            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            return jwtToken.Claims.ToDictionary(claim => claim.Type, claim => claim.Value);
        }

        public int GetUserIdFromToken()
        {
            var claims = GetClaimsByCookie();
            if (claims.ContainsKey("id"))
            {
                return int.Parse(claims["id"]);
            }
            else
            {
                throw new UnauthorizedAccessException("Token is invalid");
            }
        }

        public bool IsAdmin()
        {
            int id = GetUserIdFromToken();
            var role = _accountRepository.GetRoleById(id);
            return role == Permission.admin;
        }

        public string GetUserName()
        {
            int id = GetUserIdFromToken();
            return _accountRepository.GetUserNameById(id);
        }
    
        public Account? GetAccountById(int id)
        {
            return _accountRepository.GetAccountById(id);
        }

        public void UpdateAccount(Account account)
        {
            _accountRepository.UpdateAccount(account);
        }

        public Dictionary<int, Account> SearchAccount(string? search, int page, int limit, string role, string sortBy, string sortType)
        {
               return _accountRepository.SearchAccount(search, page, limit, role, sortBy, sortType);
        }

        public void DeleteAccount(int id)
        {
            _accountRepository.DeleteAccount(id);
        }

        public Account? GetAccountByEmail(string email)
        {
            return _accountRepository.GetAccountByEmail(email);
        }
        public void AddAccount(Account account)
        {
            _accountRepository.AddAccount(account);
        }

        public int GetTotalAccounts(string? search, string role = "user")
        {
            return _accountRepository.GetTotalAccounts(search, role);
        }
    }
}
