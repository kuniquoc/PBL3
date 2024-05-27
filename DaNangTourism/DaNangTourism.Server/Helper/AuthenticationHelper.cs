using DaNangTourism.Server.BLL;

namespace DaNangTourism.Server.Helper
{
    /// <summary>
    ///  bỏ khỏi Helper vì không đúng nên chuyển sang IAccountService
    /// </summary>
    public interface IAuthenticationHelper
    {
        int GetUserIdFromToken();
    }
    public class AuthenticationHelper : IAuthenticationHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public AuthenticationHelper(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public int GetUserIdFromToken()
        {
            HttpContext? httpContext = _httpContextAccessor.HttpContext;
            AccountBLL accountBLL = AccountBLL.Instance(_configuration);
            if (httpContext == null)
            {
                throw new UnauthorizedAccessException("HttpContext is missing");
            }
            if (httpContext.Request.Cookies.ContainsKey("jwtToken"))
            {
                var claims = accountBLL.GetClaimsByCookie(httpContext);
                if (claims.ContainsKey("id"))
                {
                    return int.Parse(claims["id"]);
                }
                else
                {
                    throw new UnauthorizedAccessException("Token is invalid");
                }
            }
            else
            {
                throw new UnauthorizedAccessException("Token is missing");
            }
        }
    }
}
