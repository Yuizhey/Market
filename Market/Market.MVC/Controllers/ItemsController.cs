using Microsoft.AspNetCore.Mvc;

namespace Market.MVC.Controllers;

public class ItemsController : Controller
{
    public ItemsController()
    {
        
    }

    [HttpGet]
    public IActionResult Index([FromQuery]string type)
    {
        if (type == "grid")
        {
            return View("ItemsGrid");
        }
        else
        {
            return View("ItemsList");
        }
    }

    [HttpGet("items/{id}")]
    public IActionResult Details([FromRoute] int id)
    {
        return View("SingleItem");
    }
}