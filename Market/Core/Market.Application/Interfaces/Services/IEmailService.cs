namespace Market.Application.Interfaces.Services;

public interface IEmailService
{
    Task SendEmailAsync(string email, string subject, string message);
}