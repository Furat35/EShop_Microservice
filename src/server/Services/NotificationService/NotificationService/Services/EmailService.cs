using Microsoft.Extensions.Options;
using NotificationService.Models;
using NotificationService.Services.Interfaces;
using System.Net;
using System.Net.Mail;

namespace NotificationService.Services
{
    public class EmailService(IOptions<SmtpConfiguration> smtpConfig) : IEmailService
    {
        private readonly SmtpConfiguration _smptConfig = smtpConfig.Value;

        public async Task SendAsync(string to, string subject, string body)
        {
            SmtpClient client = new()
            {
                Port = _smptConfig.Port,
                Host = _smptConfig.Host,
                //EnableSsl = true,
                //DeliveryMethod = SmtpDeliveryMethod.Network,
                //UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smptConfig.SenderEmail, _smptConfig.Password)
            };

            await client.SendMailAsync(_smptConfig.SenderEmail, to, subject, body);
        }
    }
}
