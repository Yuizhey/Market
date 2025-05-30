using Market.Application.Features.Auth.Register;
using Market.MVC.Models.Auth;
using MediatR;
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
            FullName = model.FullName,
        });

        if (result)
        {
            return RedirectToAction("Index", "Home");
        }
        
        ModelState.AddModelError(string.Empty, "Email or password is incorrect.");
        return View(model);
    }
}