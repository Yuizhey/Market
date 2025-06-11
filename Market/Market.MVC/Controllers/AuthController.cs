using Market.Application.Features.Auth.Comands.AuthorRegister;
using Market.Application.Features.Auth.Comands.Logout;
using Market.Application.Features.Auth.Queries.Login;
using Market.Application.Features.Auth.Register;
using Market.MVC.Models.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.MVC.Controllers;

public class AuthController : Controller
{
    private readonly IMediator _mediator;
    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("login")]
    public IActionResult Login()
    {
        return View("Login");
    }
    
    [HttpGet("register")]
    public IActionResult Register()
    {
        return View("Register");
    }
    
    [HttpGet("author-register")]
    public IActionResult AuthorRegister()
    {
        return View("AuthorRegister");
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        var result = await _mediator.Send(new RegisterCommand
        {
            Email = model.Email,
            Password = model.Password,
            ConfirmPassword = model.ConfirmPassword,
            FullName = model.UserName,
        });

        if (result)
        {
            return RedirectToAction("Index", "Admin");
        }
        
        ModelState.AddModelError(string.Empty, "Email or password is incorrect.");
        return View(model);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var result = await _mediator.Send(new LoginQuery
        {
            Email = model.Email,
            Password = model.Password
        });

        if (result)
        {
            return RedirectToAction("Index", "Admin");
        }
        
        ModelState.AddModelError(string.Empty, "Email or password is incorrect.");
        return View(model);
    }
    
    [HttpPost("author-register")]
    public async Task<IActionResult> AuthorRegister(AuthorRegisterViewModel model)
    {
        var result = await _mediator.Send(new AuthorRegisterCommand
        {
            Email = model.Email,
            Password = model.Password,
            AuthorUserName = model.AuthorUserName,
        });

        if (result)
        {
            return RedirectToAction("Index", "Admin");
        }
        
        ModelState.AddModelError(string.Empty, "Email or password is incorrect.");
        return View(model);
    }
    
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _mediator.Send(new LogoutCommand());
        return RedirectToAction("Index", "Admin");
    }
}