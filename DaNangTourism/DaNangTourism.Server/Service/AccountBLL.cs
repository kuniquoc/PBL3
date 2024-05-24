using DaNangTourism.Server.DAL;
using DaNangTourism.Server.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace DaNangTourism.Server.BLL
{
    public class AccountBLL
    {
        private readonly IConfiguration _configuration;

        private readonly AccountDAO _accountDAO;

        private AccountBLL(IConfiguration configuration)
        {
            _configuration = configuration;
            _accountDAO = AccountDAO.Instance;
        }

        private static AccountBLL? _instance;

        public static AccountBLL Instance(IConfiguration configuration)
        {
            if (_instance == null)
            {
                _instance = new AccountBLL(configuration);
            }
            return _instance;
        }

        //Lấy claims theo cookie
        public Dictionary<string, string> GetClaimsByCookie(HttpContext httpContext)
        {
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
        public string GenerateJwtToken(Account account, HttpResponse response)
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

            // Tạo và thiết lập cookie
            SetJwtTokenCookie(jwt, response);

            return jwt;
        }

        public void SetJwtTokenCookie(string token, HttpResponse response)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(1)
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
                throw new Exception("Token not found in cookies");
            }
        }

    }
}
