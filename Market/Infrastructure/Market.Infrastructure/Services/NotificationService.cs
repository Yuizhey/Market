using Market.Application.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;

namespace Market.Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly IHubContext<NotificationHub> _hub;

    public NotificationService(IHubContext<NotificationHub> hub)
    {
        _hub = hub;
    }

    public async Task SendMessage(string userId, string message)
    {
        await _hub.Clients.User(userId).SendAsync("ReceiveMessage", message);
    }

    public async Task Broadcast(string message)
    {
        await _hub.Clients.All.SendAsync("ReceiveBroadcast", message);
    }
}