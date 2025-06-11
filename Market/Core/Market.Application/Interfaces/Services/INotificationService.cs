namespace Market.Application.Interfaces.Services;

public interface INotificationService
{
    Task SendMessage(string userId, string message);
    Task Broadcast(string message);
    Task SendToRole(string role, string message);
    Task SendToUnauthenticated(string message);
}