using Market.MVC.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Market.MVC.Controllers;

public class AuthController : Controller
{
    public AuthController()
    {
        
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
        return View(model);
    }
}