using Market.Application.Features.Products.Queries.GetAll;
using Market.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Market.MVC.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = nameof(UserRoles.Admin))]
public class ProductsController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IMediator mediator, ILogger<ProductsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        _logger.LogInformation("Просмотр списка товаров администратором");
        
        var query = new GetAllProductsQuery();
        var products = await _mediator.Send(query);
        _logger.LogInformation("Загружено {Count} товаров", products.Count());
        
        return View(products);
    }
} 