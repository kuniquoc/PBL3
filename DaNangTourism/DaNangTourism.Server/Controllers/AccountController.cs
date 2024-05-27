using DaNangTourism.Server.BLL;
using DaNangTourism.Server.DAL;
using DaNangTourism.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using DaNangTourism.Server.Service;
using System.Text;
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

        //FOR USER
        /* Lấy tài khoản theo cookie
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
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Unauthorized access" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
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
        [HttpPut("update")]
        public ActionResult UpdateAccount([FromBody] AccountUpdateModel account)
        {
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
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Unauthorized access" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        //FOR ADMIN
        /* User list for admin to manage (cookie required and admin permission)
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
                int total = result.Count;

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
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Unauthorized access");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
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
                account.Permission = Permission.admin;
                _accountService.UpdateAccount(account);

                var response = new
                {
                    message = "Update user role successful"
                };

                return Ok(response);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Unauthorized access");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
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
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Unauthorized access");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        #endregion

    }
}
