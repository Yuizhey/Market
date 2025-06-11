using Market.Application.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;



public class NotificationService : INotificationService
{
    private readonly IHubContext<NotificationHub> _hub;

    public NotificationService(IHubContext<NotificationHub> hub)
    {
        _hub = hub;
    }

    public async Task SendMessage(string userId, string message)
    {
        await _hub.Clients.Group($"user:{userId}").SendAsync("ReceiveMessage", message);
    }

    public async Task Broadcast(string message)
    {
        await _hub.Clients.All.SendAsync("ReceiveBroadcast", message);
    }
    
    public async Task SendToRole(string role, string message)
    {
        await _hub.Clients.Group(role).SendAsync("ReceiveByRole", message);
    }

    public async Task SendToUnauthenticated(string message)
    {
        await _hub.Clients.Group("unauthenticated").SendAsync("ReceiveUnauthenticated", message);
    }
}