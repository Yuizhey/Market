using Market.Application.Features.Products.Commands;
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
    public async Task<IActionResult> Index([FromQuery] string type, [FromQuery] int page = 1)
    {
        var pageSize = type == "grid" ? 15 : 9;
        var query = new GetByPageNumberQuery(page, pageSize);
        var result = await _mediator.Send(query);

        var viewModel = new ItemsVM
        {
            Products = result.Products,
            TotalPages = result.TotalPages,
            CurrentPage = result.CurrentPage
        };

        return View(type == "grid" ? "ItemsGrid" : "ItemsList", viewModel);
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

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] AddItemVM viewModel)
    {
        var command = new AddNewProductCommand
        {
            Title = viewModel.Title,
            Price = viewModel.Price,
            Text = viewModel.Text,
            CoverImage = viewModel.CoverImage
        };
        await _mediator.Send(command);
        return RedirectToAction("Index");
    }
}