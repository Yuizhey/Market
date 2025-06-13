using Market.Application.Features.Products.Commands;
using Market.Application.Features.Products.Queries.GetAdditionalFiles;
using Market.Application.Features.Products.Queries.GetByPageNumber;
using Market.Application.Features.Products.Queries.GetByProductId;
using Market.Application.Interfaces.Repositories;
using Market.Application.Interfaces.Services;
using Market.Domain.Enums;
using Market.Infrastructure.Services;
using Market.MVC.Models.Items;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Market.MVC.Controllers;

public class ItemsController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<ItemsController> _logger;

    public ItemsController(IMediator mediator, ILogger<ItemsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Index(
        [FromQuery] string type, 
        [FromQuery] int page = 1, 
        [FromQuery] string? productTypes = null,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null)
    {
        _logger.LogInformation("Просмотр списка товаров. Тип отображения: {Type}, Страница: {Page}", type, page);
        
        var pageSize = type == "grid" ? 15 : 9;
        
        IEnumerable<ProductType>? selectedTypes = null;
        if (!string.IsNullOrEmpty(productTypes))
        {
            selectedTypes = productTypes.Split(',')
                .Select(t => Enum.Parse<ProductType>(t))
                .ToList();
        }

        var query = new GetByPageNumberQuery(page, pageSize, selectedTypes, minPrice, maxPrice);
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
    [AllowAnonymous]
    public async Task<IActionResult> Details(Guid id)
    {
        _logger.LogInformation("Просмотр деталей товара с ID: {ProductId}", id);
        
        var query = new GetByProductIdQuery(id);
        var product = await _mediator.Send(query);
        if (product == null)
        {
            _logger.LogWarning("Товар с ID {ProductId} не найден", id);
            return NotFound();
        }

        var viewModel = new SingleItemVM
        {
            Id = product.Id,
            Title = product.Title,
            Price = product.Price,
            Text = product.Text,
            ImageURL = product.ImageURL,
            LikesCount = product.LikesCount,
            IsLiked = product.IsLiked,
        };
        return View("SingleItem", viewModel);
    }

    [HttpPost]
    [Authorize(Roles = nameof(UserRoles.AuthorUser))]
    public async Task<IActionResult> Create([FromForm] AddItemVM viewModel)
    {
        _logger.LogInformation("Создание нового товара: {Title}", viewModel.Title);
        
        var command = new AddNewProductCommand
        {
            Title = viewModel.Title,
            Price = viewModel.Price,
            Text = viewModel.Text,
            CoverImage = viewModel.CoverImage,
            AdditionalFiles = viewModel.AdditionalFiles,
            Subtitle = viewModel.Subtitle,
            ShortDescription = viewModel.ShortDescription,
            ProductType = viewModel.ProductType,
        };
        await _mediator.Send(command);
        _logger.LogInformation("Товар успешно создан: {Title}", viewModel.Title);
        return RedirectToAction("Index");
    }

    [HttpDelete("Items/Delete/{id}")]
    [Authorize(Roles = nameof(UserRoles.AuthorUser))]
    public async Task<IActionResult> Delete(Guid id)
    {
        _logger.LogInformation("Удаление товара с ID: {ProductId}", id);
        await _mediator.Send(new DeleteProductCommand(id));
        _logger.LogInformation("Товар успешно удален: {ProductId}", id);
        return Ok();
    }
    
    [HttpGet]
    [Authorize(Roles = $"{nameof(UserRoles.CLientUser)},{nameof(UserRoles.AuthorUser)}")]
    public async Task<IActionResult> DownloadAdditionalFiles(Guid productId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Скачивание дополнительных файлов для товара: {ProductId}", productId);
        var query = new GetAdditionalFilesUrlsQuery(productId);
        var zipFile = await _mediator.Send(query, cancellationToken);
        if (zipFile == null || zipFile.Length == 0)
        {
            return NotFound("Файлы для скачивания отсутствуют.");
        }

        return File(zipFile, "application/zip", $"AdditionalFiles_{productId}.zip");
    }

}