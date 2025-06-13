using Market.Application.Features.Users.Commands.CreateAdmin;
using Market.Application.Features.Users.Commands.DeleteAdmin;
using Market.Application.Features.Users.Queries.GetAdmins;
using Market.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.MVC.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = nameof(UserRoles.Admin))]
public class AdminsController : Controller
{
    private readonly IMediator _mediator;

    public AdminsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index()
    {
        var admins = await _mediator.Send(new GetAdminsQuery());
        return View(admins);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string email, string password)
    {
        await _mediator.Send(new CreateAdminCommand(email, password));
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteAdminCommand(id));
        return RedirectToAction(nameof(Index));
    }
} 