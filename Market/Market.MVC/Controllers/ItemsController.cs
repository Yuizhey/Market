using Market.Application.Features.Products.Commands;
using Market.Application.Features.Products.Queries.GetByPageNumber;
using Market.Application.Features.Products.Queries.GetByProductId;
using Market.Domain.Enums;
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
    public async Task<IActionResult> Index([FromQuery] string type, [FromQuery] int page = 1, [FromQuery] string? productTypes = null)
    {
        var pageSize = type == "grid" ? 15 : 9;
        
        IEnumerable<ProductType>? selectedTypes = null;
        if (!string.IsNullOrEmpty(productTypes))
        {
            selectedTypes = productTypes.Split(',')
                .Select(t => Enum.Parse<ProductType>(t))
                .ToList();
        }

        var query = new GetByPageNumberQuery(page, pageSize, selectedTypes);
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
            ImageURL = product.ImageURL,
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