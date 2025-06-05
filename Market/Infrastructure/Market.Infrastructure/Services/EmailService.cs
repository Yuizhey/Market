using System.Net;
using System.Net.Mail;
using Market.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace Market.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly SmtpClient _smtpClient;
    private readonly string _fromEmail;

    public EmailService(IConfiguration configuration)
    {
        _fromEmail = configuration["EmailSettings:FromEmail"];
        var username = configuration["EmailSettings:Username"];
        var password = configuration["EmailSettings:Password"];
        var smtpServer = configuration["EmailSettings:SmtpServer"];
        var port = int.Parse(configuration["EmailSettings:Port"]);
        
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        _smtpClient = new SmtpClient(smtpServer, port)
        {
            Credentials = new NetworkCredential(username, password),
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Timeout = 10000
        };
    }


    public async Task SendEmailAsync(string email, string subject, string message)
    {
        try
        {
            using var mailMessage = new MailMessage
            {
                From = new MailAddress(_fromEmail),
                Subject = subject,
                Body = message,
                IsBodyHtml = false
            };
            mailMessage.To.Add(email);

            await _smtpClient.SendMailAsync(mailMessage);
        }
        catch (SmtpException ex)
        {
            Console.WriteLine($"SMTP Error: {ex.StatusCode} - {ex.Message}");
            throw new ApplicationException($"Failed to send email: {ex.Message}", ex);
        }
    }
}