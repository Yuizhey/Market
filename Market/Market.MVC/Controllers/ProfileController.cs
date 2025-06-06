using System.Security.Claims;
using Market.Application.Features.Products.Queries.GetByUserId;
using Market.Application.Features.Profile.Commands.AddAuthorUserDescription;
using Market.Application.Features.Profile.Commands.AddUserDescription;
using Market.Application.Features.Purchase.Queries.GetUserPurchases;
using Market.Domain.Enums;
using Market.MVC.Models.Profile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.MVC.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly IMediator _mediator;

    public ProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public IActionResult Index()
    {
        return View();
    }
    
    [Authorize(Roles = nameof(UserRoles.AuthorUser))]
    public IActionResult AddNewItem()
    {
        return View();
    }
    
    [HttpPost]
    [Authorize(Roles = nameof(UserRoles.CLientUser))]
    public async Task<IActionResult> AddUserDescription(UserDescriptionVM vm)
    {
        if (!ModelState.IsValid)
            return View("PrifileSettings", vm);

        var command = new AddUserDescriptionCommand
        {
            FirstName = vm.FirstName,
            LastName = vm.LastName,
            Phone = vm.Phone,
            Gender = vm.Gender
        };

        await _mediator.Send(command);
        return RedirectToAction("Index");
    }

    [HttpPost]
    [Authorize(Roles = nameof(UserRoles.AuthorUser))]
    public async Task<IActionResult> AddAuthorUserDescription(AuthorUserDescriptionVM vm)
    {
        if (!ModelState.IsValid)
            return View("PrifileSettings", vm);

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
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public IActionResult Settings()
    {
        return View("PrifileSettings");
    }

    [HttpGet]
    [Authorize(Roles = nameof(UserRoles.AuthorUser))]
    public async Task<IActionResult> MyProducts()
    {
        var query = new GetByUserIdQuery();
        var myProducts = await _mediator.Send(query);
        var viewModel = new MyProductsVM
        {
            MyProducts = myProducts
        };
        return View(viewModel);
    }
    
    [HttpGet]
    public async Task<IActionResult> MyDownloads()
    {
        var purchases = await _mediator.Send(new GetUserPurchasesQuery());
        var viewmodel = new MyDownloadsVM
        {
            Downloads = purchases
        };
        return View(viewmodel);
    }
}