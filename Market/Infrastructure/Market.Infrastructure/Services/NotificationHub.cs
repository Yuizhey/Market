using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var user = Context.User;

        if (user?.Identity?.IsAuthenticated ?? false)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = user.FindFirst(ClaimTypes.Role)?.Value;

            if (!string.IsNullOrEmpty(role))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, role);
            }
            
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user:{userId}");
            }
        }
        else
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "unauthenticated");
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var user = Context.User;

        if (user?.Identity?.IsAuthenticated ?? false)
        {
            var role = user.FindFirst(ClaimTypes.Role)?.Value;
            if (!string.IsNullOrEmpty(role))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, role);
            }
        }
        else
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "unauthenticated");
        }

        await base.OnDisconnectedAsync(exception);
    }
}