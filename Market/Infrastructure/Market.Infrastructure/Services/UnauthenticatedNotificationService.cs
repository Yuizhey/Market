using Market.Application.Interfaces.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Market.Infrastructure.Services;

public class UnauthenticatedNotificationService : BackgroundService
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<UnauthenticatedNotificationService> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(1);

    public UnauthenticatedNotificationService(
        INotificationService notificationService,
        ILogger<UnauthenticatedNotificationService> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await _notificationService.SendToUnauthenticated(
                    "Для совершения покупок необходимо войти в свой аккаунт. Пожалуйста, авторизуйтесь для продолжения."
                );
                
                _logger.LogInformation("Отправлено уведомление неавторизованным пользователям");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при отправке уведомления неавторизованным пользователям");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }
} 