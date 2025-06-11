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
                ModelState.AddModelError("", "Сообщение не может быть пустым");
                return View("Index");
            }

            await _notificationService.Broadcast(message);
            ViewBag.Success = "Сообщение отправлено всем пользователям!";
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> SendToUser(string message, string userId)
        {
            if (string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError("", "Сообщение не может быть пустым");
                return View("Index");
            }

            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError("", "ID пользователя не может быть пустым");
                return View("Index");
            }

            await _notificationService.SendMessage(userId, message);
            ViewBag.Success = $"Сообщение отправлено пользователю {userId}!";
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> SendToRole(string message, string role)
        {
            if (string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError("", "Сообщение не может быть пустым");
                return View("Index");
            }

            if (string.IsNullOrEmpty(role))
            {
                ModelState.AddModelError("", "Роль не может быть пустой");
                return View("Index");
            }

            await _notificationService.SendToRole(role, message);
            ViewBag.Success = $"Сообщение отправлено роли {role}!";
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> SendToUnauthenticated(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError("", "Сообщение не может быть пустым");
                return View("Index");
            }

            await _notificationService.SendToUnauthenticated(message);
            ViewBag.Success = "Сообщение отправлено неавторизованным пользователям!";
            return View("Index");
        }
    }
}