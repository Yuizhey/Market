using Market.Application.Interfaces;
using Market.Application.Interfaces.Services;
using Market.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Market.Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(IHubContext<NotificationHub> hubContext, ILogger<NotificationService> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task SendNotificationAsync(string userId, string message)
    {
        _logger.LogInformation("Отправка уведомления пользователю {UserId}: {Message}", userId, message);
        
        try
        {
            await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", message);
            _logger.LogInformation("Уведомление успешно отправлено пользователю {UserId}", userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при отправке уведомления пользователю {UserId}", userId);
        }
    }

    public async Task SendMessage(string userId, string message)
    {
        await _hubContext.Clients.Group($"user:{userId}").SendAsync("ReceiveMessage", message);
    }

    public async Task Broadcast(string message)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveBroadcast", message);
    }
    
    public async Task SendToRole(string role, string message)
    {
        await _hubContext.Clients.Group(role).SendAsync("ReceiveByRole", message);
    }

    public async Task SendToUnauthenticated(string message)
    {
        await _hubContext.Clients.Group("unauthenticated").SendAsync("ReceiveUnauthenticated", message);
    }
}