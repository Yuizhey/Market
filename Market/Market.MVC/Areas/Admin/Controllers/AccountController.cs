using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Market.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Market.MVC.Areas.Admin.Controllers;

[Area("Admin")]
public class AccountController : Controller
{
    private readonly IAuthService _authService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IAuthService authService, ILogger<AccountController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Login()
    {
        _logger.LogInformation("Просмотр страницы входа администратора");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        _logger.LogInformation("Попытка входа администратора с email: {Email}", email);
        
        var result = await _authService.AdminLoginAsync(email, password);
        if (result)
        {
            _logger.LogInformation("Успешный вход администратора: {Email}", email);
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }

        _logger.LogWarning("Неудачная попытка входа администратора: {Email}", email);
        ModelState.AddModelError("", "Неверный email или пароль");
        return View();
    }

    public async Task<IActionResult> Logout()
    {
        _logger.LogInformation("Выход администратора");
        await _authService.LogoutAsync();
        return RedirectToAction("Login");
    }
} 