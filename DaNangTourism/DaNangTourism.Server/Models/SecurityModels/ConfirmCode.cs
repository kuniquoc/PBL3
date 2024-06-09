using System.Text;
using MySqlConnector;

namespace DaNangTourism.Server.Models.SecurityModels
{
  public class ConfirmCode
  {
    public string Code { get; set; } = "";
    public string Email { get; set; } = "";
    public DateTime ExpiredAt { get; set; } = DateTime.Now;

    public ConfirmCode() { }

    public ConfirmCode(string email, int expiredAfter)
    {
      Email = email;
      Code = CreateConfirmationCode();
      ExpiredAt = DateTime.Now.AddMinutes(expiredAfter);
    }

    public ConfirmCode(MySqlDataReader reader)
    {
      Email = reader.GetString("email");
      Code = reader.GetString("code");
      ExpiredAt = reader.GetDateTime("expired_at");
    }

    private static string CreateConfirmationCode()
    {
      var code = new StringBuilder();
      var random = new Random();
      for (int i = 0; i < 6; i++)
      {
        code.Append(random.Next(0, 9));
      }
      return code.ToString();
    }
  }
}