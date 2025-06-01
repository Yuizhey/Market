using Microsoft.AspNetCore.Mvc;

namespace Market.MVC.Controllers;

public class ProfileController : Controller
{
    public ProfileController()
    {
        
    }

    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult AddNewItem()
    {
        return View();
    }
}