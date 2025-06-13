using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Market.Application.Interfaces.Services;
using Market.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Market.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = nameof(UserRoles.Admin))]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(INotificationService notificationService, ILogger<NotificationController> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogInformation("Просмотр страницы уведомлений администратором");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendBroadcast(string message)
        {
            _logger.LogInformation("Попытка отправки широковещательного сообщения: {Message}", message);
            
            if (string.IsNullOrEmpty(message))
            {
                _logger.LogWarning("Попытка отправки пустого сообщения");
                return Json(new { success = false, message = "Сообщение не может быть пустым" });
            }

            await _notificationService.Broadcast(message);
            _logger.LogInformation("Широковещательное сообщение успешно отправлено");
            return Json(new { success = true, message = "Сообщение отправлено всем пользователям!" });
        }

        [HttpPost]
        public async Task<IActionResult> SendToUser(string message, string userId)
        {
            _logger.LogInformation("Попытка отправки сообщения пользователю {UserId}: {Message}", userId, message);
            
            if (string.IsNullOrEmpty(message))
            {
                _logger.LogWarning("Попытка отправки пустого сообщения пользователю {UserId}", userId);
                return Json(new { success = false, message = "Сообщение не может быть пустым" });
            }

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("Попытка отправки сообщения без указания пользователя");
                return Json(new { success = false, message = "ID пользователя не может быть пустым" });
            }

            await _notificationService.SendMessage(userId, message);
            _logger.LogInformation("Сообщение успешно отправлено пользователю {UserId}", userId);
            return Json(new { success = true, message = $"Сообщение отправлено пользователю {userId}!" });
        }

        [HttpPost]
        public async Task<IActionResult> SendToRole(string message, string role)
        {
            _logger.LogInformation("Попытка отправки сообщения роли {Role}: {Message}", role, message);
            
            if (string.IsNullOrEmpty(message))
            {
                _logger.LogWarning("Попытка отправки пустого сообщения роли {Role}", role);
                return Json(new { success = false, message = "Сообщение не может быть пустым" });
            }

            if (string.IsNullOrEmpty(role))
            {
                _logger.LogWarning("Попытка отправки сообщения без указания роли");
                return Json(new { success = false, message = "Роль не может быть пустой" });
            }

            await _notificationService.SendToRole(role, message);
            _logger.LogInformation("Сообщение успешно отправлено роли {Role}", role);
            return Json(new { success = true, message = $"Сообщение отправлено роли {role}!" });
        }

        [HttpPost]
        public async Task<IActionResult> SendToUnauthenticated(string message)
        {
            _logger.LogInformation("Попытка отправки сообщения неавторизованным пользователям: {Message}", message);
            
            if (string.IsNullOrEmpty(message))
            {
                _logger.LogWarning("Попытка отправки пустого сообщения неавторизованным пользователям");
                return Json(new { success = false, message = "Сообщение не может быть пустым" });
            }

            await _notificationService.SendToUnauthenticated(message);
            _logger.LogInformation("Сообщение успешно отправлено неавторизованным пользователям");
            return Json(new { success = true, message = "Сообщение отправлено неавторизованным пользователям!" });
        }
    }
}