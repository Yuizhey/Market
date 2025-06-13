using System.Net;
using System.Net.Mail;
using Market.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Market.Application.Interfaces.Services;

namespace Market.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly SmtpClient _smtpClient;
    private readonly string _fromEmail;
    private readonly ILogger<EmailService> _logger;
    private const int MaxRetries = 3;
    private const int RetryDelayMs = 1000;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _logger = logger;
        _fromEmail = configuration["EmailSettings:FromEmail"];
        var username = configuration["EmailSettings:Username"];
        var password = configuration["EmailSettings:Password"];
        var smtpServer = configuration["EmailSettings:SmtpServer"];
        var port = int.Parse(configuration["EmailSettings:Port"]);
        
        _logger.LogInformation("Инициализация EmailService. SMTP сервер: {Server}, порт: {Port}", smtpServer, port);
        
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        _smtpClient = new SmtpClient(smtpServer, port)
        {
            Credentials = new NetworkCredential(username, password),
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Timeout = 5000 // Уменьшаем таймаут до 5 секунд
        };
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        _logger.LogInformation("Начало отправки email на адрес: {Email}, тема: {Subject}", email, subject);
        
        for (int attempt = 1; attempt <= MaxRetries; attempt++)
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

                _logger.LogInformation("Попытка {Attempt} отправки email", attempt);
                await _smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation("Email успешно отправлен на {Email}", email);
                return; // Успешная отправка
            }
            catch (SmtpException ex)
            {
                _logger.LogWarning(ex, "Попытка {Attempt} отправки email на {Email} не удалась. Ошибка: {Error}", 
                    attempt, email, ex.Message);
                
                if (attempt == MaxRetries)
                {
                    _logger.LogError(ex, "Не удалось отправить email на {Email} после {MaxRetries} попыток", 
                        email, MaxRetries);
                    // Не выбрасываем исключение, просто логируем ошибку
                    return;
                }
                
                _logger.LogInformation("Ожидание {Delay}мс перед следующей попыткой", RetryDelayMs * attempt);
                await Task.Delay(RetryDelayMs * attempt); // Увеличиваем задержку с каждой попыткой
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Непредвиденная ошибка при отправке email на {Email}", email);
                return; // Не выбрасываем исключение при неожиданных ошибках
            }
        }
    }
}