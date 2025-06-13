using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

public class NotificationHub : Hub
{
    private readonly ILogger<NotificationHub> _logger;

    public NotificationHub(ILogger<NotificationHub> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("Новое подключение к хабу. ConnectionId: {ConnectionId}", Context.ConnectionId);
        
        var user = Context.User;

        if (user?.Identity?.IsAuthenticated ?? false)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = user.FindFirst(ClaimTypes.Role)?.Value;

            _logger.LogInformation("Аутентифицированный пользователь подключился. UserId: {UserId}, Role: {Role}", 
                userId, role);

            if (!string.IsNullOrEmpty(role))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, role);
                _logger.LogInformation("Пользователь {UserId} добавлен в группу роли {Role}", userId, role);
            }
            
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user:{userId}");
                _logger.LogInformation("Пользователь {UserId} добавлен в свою группу", userId);
            }
        }
        else
        {
            _logger.LogInformation("Неаутентифицированный пользователь подключился. ConnectionId: {ConnectionId}", 
                Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, "unauthenticated");
            _logger.LogInformation("Пользователь добавлен в группу неаутентифицированных");
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("Отключение от хаба. ConnectionId: {ConnectionId}", Context.ConnectionId);
        
        var user = Context.User;

        if (user?.Identity?.IsAuthenticated ?? false)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = user.FindFirst(ClaimTypes.Role)?.Value;
            
            _logger.LogInformation("Аутентифицированный пользователь отключился. UserId: {UserId}, Role: {Role}", 
                userId, role);

            if (!string.IsNullOrEmpty(role))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, role);
                _logger.LogInformation("Пользователь {UserId} удален из группы роли {Role}", userId, role);
            }
            
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user:{userId}");
                _logger.LogInformation("Пользователь {UserId} удален из своей группы", userId);
            }
        }
        else
        {
            _logger.LogInformation("Неаутентифицированный пользователь отключился. ConnectionId: {ConnectionId}", 
                Context.ConnectionId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "unauthenticated");
            _logger.LogInformation("Пользователь удален из группы неаутентифицированных");
        }

        if (exception != null)
        {
            _logger.LogError(exception, "Ошибка при отключении пользователя. ConnectionId: {ConnectionId}", 
                Context.ConnectionId);
        }

        await base.OnDisconnectedAsync(exception);
    }
}