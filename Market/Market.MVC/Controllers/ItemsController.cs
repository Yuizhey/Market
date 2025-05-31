using Market.Application.Features.Products.Queries.GetByPageNumber;
using Market.MVC.Models.Items;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Market.MVC.Controllers;

public class ItemsController : Controller
{
    private readonly IMediator _mediator;
    public ItemsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery]string type)
    {
        
        if (type == "grid")
        {
            var query = new GetByPageNumberQuery(1, 15);
            var products = await _mediator.Send(query);
            var viewModel = new ItemsVM
            {
                Products = products
            };
            return View("ItemsGrid", viewModel);
        }
        else
        {
            var query = new GetByPageNumberQuery(1, 9);
            var products = await _mediator.Send(query);
            var viewModel = new ItemsVM
            {
                Products = products
            };
            return View("ItemsList", viewModel);
        }
    }

    [HttpGet("items/{id}")]
    public IActionResult Details([FromRoute] int id)
    {
        return View("SingleItem");
    }
}