using Market.Application.Features.Auth.Comands.AuthorRegister;
using Market.Application.Features.Auth.Comands.Logout;
using Market.Application.Features.Auth.Queries.Login;
using Market.Application.Features.Auth.Register;
using Market.MVC.Models.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Market.MVC.Controllers;

public class AuthController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IMediator mediator, ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("login")]
    public IActionResult Login()
    {
        _logger.LogInformation("Пользователь открыл страницу входа");
        return View("Login");
    }
    
    [HttpGet("register")]
    public IActionResult Register()
    {
        _logger.LogInformation("Пользователь открыл страницу регистрации");
        return View("Register");
    }
    
    [HttpGet("author-register")]
    public IActionResult AuthorRegister()
    {
        _logger.LogInformation("Пользователь открыл страницу регистрации автора");
        return View("AuthorRegister");
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        _logger.LogInformation("Попытка регистрации пользователя с email: {Email}", model.Email);
        
        var result = await _mediator.Send(new RegisterCommand
        {
            Email = model.Email,
            Password = model.Password,
            ConfirmPassword = model.ConfirmPassword,
            FullName = model.UserName,
        });

        if (result)
        {
            _logger.LogInformation("Успешная регистрация пользователя с email: {Email}", model.Email);
            return RedirectToAction("Index", "Home");
        }
        
        _logger.LogWarning("Неудачная попытка регистрации пользователя с email: {Email}", model.Email);
        ModelState.AddModelError(string.Empty, "Email or password is incorrect.");
        return View(model);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        _logger.LogInformation("Попытка входа пользователя с email: {Email}", model.Email);
        
        var result = await _mediator.Send(new LoginQuery
        {
            Email = model.Email,
            Password = model.Password
        });

        if (result)
        {
            _logger.LogInformation("Успешный вход пользователя с email: {Email}", model.Email);
            return RedirectToAction("Index", "Home");
        }
        
        _logger.LogWarning("Неудачная попытка входа пользователя с email: {Email}", model.Email);
        ModelState.AddModelError(string.Empty, "Email or password is incorrect.");
        return View(model);
    }
    
    [HttpPost("author-register")]
    public async Task<IActionResult> AuthorRegister(AuthorRegisterViewModel model)
    {
        _logger.LogInformation("Попытка регистрации автора с email: {Email}", model.Email);
        
        var result = await _mediator.Send(new AuthorRegisterCommand
        {
            Email = model.Email,
            Password = model.Password,
            AuthorUserName = model.AuthorUserName,
        });

        if (result)
        {
            _logger.LogInformation("Успешная регистрация автора с email: {Email}", model.Email);
            return RedirectToAction("Index", "Home");
        }
        
        _logger.LogWarning("Неудачная попытка регистрации автора с email: {Email}", model.Email);
        ModelState.AddModelError(string.Empty, "Email or password is incorrect.");
        return View(model);
    }
    
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        _logger.LogInformation("Выход пользователя");
        await _mediator.Send(new LogoutCommand());
        return RedirectToAction("Index", "Home");
    }
}