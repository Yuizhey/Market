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
        public async Task<IActionResult> Send(string message, string userId, string role, bool sendToAll, bool sendToUnauthenticated)
        {
            if (sendToAll)
            {
                await _notificationService.Broadcast(message);
            }
            else if (sendToUnauthenticated)
            {
                await _notificationService.SendToUnauthenticated(message);
            }
            else if (!string.IsNullOrEmpty(userId))
            {
                await _notificationService.SendMessage(userId, message);
            }
            else if (!string.IsNullOrEmpty(role))
            {
                await _notificationService.SendToRole(role, message);
            }
            else
            {
                ModelState.AddModelError("", "Выберите получателя.");
                return View("Index");
            }

            ViewBag.Success = "Сообщение отправлено!";
            return View("Index");
        }
    }
}