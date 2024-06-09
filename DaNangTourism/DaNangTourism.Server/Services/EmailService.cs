using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
namespace DaNangTourism.Server.Services
{
  public interface IEmailService
  {
    Task SendEmailAsync(string email, string subject, string html);
  }
  public class EmailService : IEmailService
  {
    public async Task SendEmailAsync(string email, string subject, string html)
    {
      var fromAddress = new MailAddress("danang.tourism.pbl3@gmail.com", "Da Nang Tourism");
      var toAddress = new MailAddress(email);
      const string fromPassword = "wvtf dyxl hkdo gesf";
      const string host = "smtp.gmail.com";
      const int port = 587;
      var smtp = new SmtpClient
      {
        Host = host,
        Port = port,
        EnableSsl = true,
        DeliveryMethod = SmtpDeliveryMethod.Network,
        UseDefaultCredentials = false,
        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
      };
      var message = new MailMessage(fromAddress, toAddress)
      {
        Subject = subject,
        Body = html,
        IsBodyHtml = true
      };
      await smtp.SendMailAsync(message);
    }
  }
}