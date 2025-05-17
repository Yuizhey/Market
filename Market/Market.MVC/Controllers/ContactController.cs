using Microsoft.AspNetCore.Mvc;

namespace Market.MVC.Controllers;

public class ContactController : Controller
{
    public ContactController()
    {
        
    }

    public IActionResult Index()
    {
        return View();
    }
}