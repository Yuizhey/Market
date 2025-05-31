namespace Market.Application.Interfaces.Services;

public interface INotificationService
{
    Task SendMessage(string userId, string message);
    Task Broadcast(string message);
}