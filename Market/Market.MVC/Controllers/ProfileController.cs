using System.Security.Claims;
using Market.Application.Features.Products.Queries.GetByUserId;
using Market.Application.Features.Profile.Commands.AddAuthorUserDescription;
using Market.Application.Features.Profile.Commands.AddUserDescription;
using Market.Application.Features.Purchase.Queries.GetUserPurchases;
using Market.Application.Features.Profile.Queries.GetUserProfile;
using Market.Domain.Enums;
using Market.MVC.Models.Profile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Market.Application.Features.Products.Queries.GetSalesStatistics;
using Microsoft.Extensions.Logging;

namespace Market.MVC.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProfileController> _logger;

    public ProfileController(IMediator mediator, ILogger<ProfileController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    
    [Authorize(Roles = nameof(UserRoles.AuthorUser))]
    public IActionResult AddNewItem()
    {
        _logger.LogInformation("Просмотр страницы добавления нового товара");
        return View();
    }
    
    [HttpPost]
    [Authorize(Roles = nameof(UserRoles.CLientUser))]
    public async Task<IActionResult> AddUserDescription(UserDescriptionVM vm)
    {
        _logger.LogInformation("Попытка добавления описания пользователя: {FirstName} {LastName}", 
            vm.FirstName, vm.LastName);

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Невалидная форма описания пользователя");
            return View("PrifileSettings");
        }

        var command = new AddUserDescriptionCommand
        {
            FirstName = vm.FirstName,
            LastName = vm.LastName,
            Phone = vm.Phone,
            Gender = vm.Gender
        };

        await _mediator.Send(command);
        _logger.LogInformation("Описание пользователя успешно добавлено: {FirstName} {LastName}", 
            vm.FirstName, vm.LastName);
        return RedirectToAction("Index");
    }

    [HttpPost]
    [Authorize(Roles = nameof(UserRoles.AuthorUser))]
    public async Task<IActionResult> AddAuthorUserDescription(AuthorUserDescriptionVM vm)
    {
        _logger.LogInformation("Попытка добавления описания автора: {FirstName} {LastName}", 
            vm.FirstName, vm.LastName);

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Невалидная форма описания автора");
            return View("PrifileSettings");
        }

        var command = new AddAuthorUserDescriptionCommand
        {
            FirstName = vm.FirstName,
            LastName = vm.LastName,
            Phone = vm.Phone,
            Gender = vm.Gender,
            Country = vm.Country,
            HomeAddress = vm.HomeAddress,
            Address = vm.Address
        };

        await _mediator.Send(command);
        _logger.LogInformation("Описание автора успешно добавлено: {FirstName} {LastName}", 
            vm.FirstName, vm.LastName);
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public async Task<IActionResult> Settings()
    {
        _logger.LogInformation("Просмотр настроек профиля");
        var profile = await _mediator.Send(new GetUserProfileQuery());
        return View("PrifileSettings", profile);
    }

    [HttpGet]
    [Authorize(Roles = nameof(UserRoles.AuthorUser))]
    public async Task<IActionResult> MyProducts()
    {
        _logger.LogInformation("Просмотр списка товаров автора");
        var query = new GetByUserIdQuery();
        var myProducts = await _mediator.Send(query);
        _logger.LogInformation("Загружено {Count} товаров автора", myProducts.Count());
        
        var viewModel = new MyProductsVM
        {
            MyProducts = myProducts
        };
        return View(viewModel);
    }
    
    [HttpGet]
    public async Task<IActionResult> MyDownloads()
    {
        _logger.LogInformation("Просмотр списка загрузок пользователя");
        var purchases = await _mediator.Send(new GetUserPurchasesQuery());
        _logger.LogInformation("Загружено {Count} покупок пользователя", purchases.Count());
        
        var viewmodel = new MyDownloadsVM
        {
            Downloads = purchases
        };
        return View(viewmodel);
    }

    [Authorize(Roles = nameof(UserRoles.AuthorUser))]
    public async Task<IActionResult> MySales()
    {
        _logger.LogInformation("Просмотр статистики продаж автора");
        var statistics = await _mediator.Send(new GetSalesStatisticsQuery());
        return View(statistics);
    }
}