using Market.Application.Features.Products.Queries.GetByPageNumber;
using Market.Application.Features.Products.Queries.GetByProductId;
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

    [HttpGet("Items/Details/{id}")]
    public async Task<IActionResult> Details(Guid id)
    {
        var query = new GetByProductIdQuery(id);
        var product = await _mediator.Send(query);
        if (product == null)
            return NotFound();

        var viewModel = new SingleItemVM
        {
            Title = product.Title,
            Price = product.Price,
            Text = product.Text,
        };
        return View("SingleItem", viewModel);
    }
}