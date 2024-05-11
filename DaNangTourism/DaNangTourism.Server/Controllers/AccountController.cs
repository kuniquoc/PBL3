using DaNangTourism.Server.DAL;
using DaNangTourism.Server.Models;
using Microsoft.AspNetCore.Mvc;


namespace DaNangTourism.Server.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController : Controller
    {
        [HttpGet("get/all")]
        public IActionResult GetAllAccounts()
        {
            AccountDAO accountDAO = new AccountDAO();
            Dictionary<int, Account> accounts = accountDAO.GetAllAccounts();
            if (accounts.Count == 0)
            {
                return NotFound();
            }
            else return Ok(accounts); 
        }

        [HttpGet("get/{id}")]
        public IActionResult GetAccountById(int id)
        {
            AccountDAO accountDAO = new AccountDAO();
            Account account = accountDAO.GetAccountById(id);
            if (account == null)
            {
                return NotFound();
            }
            else return Ok(account);
        }

        [HttpPost("add")]
        public IActionResult AddAccount([FromBody] Account account)
        {
            AccountDAO accountDAO = new AccountDAO();
            int check = accountDAO.AddAccount(account);
            if (check > 0)
            {
                return Ok();
            }
            else return BadRequest();
        }

        [HttpPut("update")]
        public IActionResult UpdateAccountInformation([FromBody] Account account)
        {
            AccountDAO accountDAO = new AccountDAO();
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
            AccountDAO accountDAO = new AccountDAO();
            int check = accountDAO.DeleteAccount(id);
            if (check > 0)
            {
                return NoContent();
            }
            else return BadRequest();
        }

        [HttpPost("login")]
        public IActionResult CheckLogin(string username, string password)
        {
            AccountDAO accountDAO = new AccountDAO();
            Account? check = accountDAO.CheckLogin(username, password);
            if (check != null)
            {
                return Ok(check);
            }
            else return NotFound();
        }

        [HttpPost("register")]
        public IActionResult CheckRegister(string username, string email)
        {
            AccountDAO accountDAO = new AccountDAO();
            int check = accountDAO.CheckRegister(username, email);
            if (check == 0)
            {
                return Ok();
            }
            else return BadRequest();
        }

        [HttpGet("check/{username}")]
        public IActionResult CheckAccountExist(string username)
        {
            AccountDAO accountDAO = new AccountDAO();
            bool check = accountDAO.CheckAccountExist(username);
            if (check)
            {
                return Ok();
            }
            else return NotFound();
        }

        [HttpPut("change/email")]
        public IActionResult ChangeEmail(int id, string email)
        {
            AccountDAO accountDAO = new AccountDAO();
            int check = accountDAO.ChangeEmail(id, email);
            if (check > 0)
            {
                return NoContent();
            }
            else return BadRequest();
        }

        [HttpPut("change/password")]
        public IActionResult ChangePassword(int id, string password)
        {
            AccountDAO accountDAO = new AccountDAO();
            int check = accountDAO.ChangePassword(id, password);
            if (check > 0)
            {
                return NoContent();
            }
            else return BadRequest();
        }
    }
}
