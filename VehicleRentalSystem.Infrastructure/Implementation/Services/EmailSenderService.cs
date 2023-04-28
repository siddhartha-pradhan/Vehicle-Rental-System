using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using VehicleRentalSystem.Domain.Constants;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace VehicleRentalSystem.Infrastructure.Implementation.Services;

public class EmailSenderService : IEmailSender
{
    private EmailSettings _emailSettings { get; }

    public EmailSenderService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }
    
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        string mail = _emailSettings.Email;
        string token = _emailSettings.Token;

        var message = new MailMessage();

        message.From = new MailAddress(mail);
        message.Subject = subject;
        message.To.Add(new MailAddress(email));
        message.Body = "<html><body> " + htmlMessage + " </body></html>";
        message.IsBodyHtml = true;

        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(mail, token),
            EnableSsl = true,
        };

        smtpClient.Send(message);
    }
}
