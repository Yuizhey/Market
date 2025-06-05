using System.Security.Claims;
using Market.Application.Features.Products.Queries.GetByUserId;
using Market.Application.Features.Profile.Commands.AddAuthorUserDescription;
using Market.Application.Features.Profile.Commands.AddUserDescription;
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
    
    public IActionResult AddNewItem()
    {
        return View();
    }
    
    [HttpPost]
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
}