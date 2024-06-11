using System.Text;
using MySqlConnector;

namespace DaNangTourism.Server.Models.SecurityModels
{
  public class InputConfirmCode
  {
    public string Email { get; set; } = "";
    public string Code { get; set; } = "";

    public InputConfirmCode() { }

    public InputConfirmCode(string email, string code)
    {
      Email = email;
      Code = code;
    }
  }

  public class InputCreateCode
  {
    public required string Email { get; set; }
    public bool IsRegister { get; set; } = true;
  }
}