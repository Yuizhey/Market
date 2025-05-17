using Microsoft.AspNetCore.Mvc;

namespace Market.MVC.Controllers;

public class CartController : Controller
{
    public CartController()
    {
        
    }

    public IActionResult Index()
    {
        return View();
    }
}