using Market.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Market.MVC.Controllers;

public class AdminController : Controller
{
    private readonly INotificationService _notificationService;

    public AdminController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public IActionResult Notify()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SendBroadcast([FromBody] string message)
    {
        await _notificationService.Broadcast(message);
        return Ok();
    }
}
