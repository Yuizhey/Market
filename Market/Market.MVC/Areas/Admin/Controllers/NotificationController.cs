using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Market.Application.Interfaces.Services;

namespace Market.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendBroadcast(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return Json(new { success = false, message = "Сообщение не может быть пустым" });
            }

            await _notificationService.Broadcast(message);
            return Json(new { success = true, message = "Сообщение отправлено всем пользователям!" });
        }

        [HttpPost]
        public async Task<IActionResult> SendToUser(string message, string userId)
        {
            if (string.IsNullOrEmpty(message))
            {
                return Json(new { success = false, message = "Сообщение не может быть пустым" });
            }

            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "ID пользователя не может быть пустым" });
            }

            await _notificationService.SendMessage(userId, message);
            return Json(new { success = true, message = $"Сообщение отправлено пользователю {userId}!" });
        }

        [HttpPost]
        public async Task<IActionResult> SendToRole(string message, string role)
        {
            if (string.IsNullOrEmpty(message))
            {
                return Json(new { success = false, message = "Сообщение не может быть пустым" });
            }

            if (string.IsNullOrEmpty(role))
            {
                return Json(new { success = false, message = "Роль не может быть пустой" });
            }

            await _notificationService.SendToRole(role, message);
            return Json(new { success = true, message = $"Сообщение отправлено роли {role}!" });
        }

        [HttpPost]
        public async Task<IActionResult> SendToUnauthenticated(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return Json(new { success = false, message = "Сообщение не может быть пустым" });
            }

            await _notificationService.SendToUnauthenticated(message);
            return Json(new { success = true, message = "Сообщение отправлено неавторизованным пользователям!" });
        }
    }
}