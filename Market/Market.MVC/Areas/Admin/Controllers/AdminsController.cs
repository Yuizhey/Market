using Market.Application.Features.Users.Commands.CreateAdmin;
using Market.Application.Features.Users.Commands.DeleteAdmin;
using Market.Application.Features.Users.Queries.GetAdmins;
using Market.Domain.Enums;
using Market.MVC.Areas.Admin.Models;
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
    public async Task<IActionResult> Create([FromBody] CreateAdminRequest request)
    {
        try
        {
            await _mediator.Send(new CreateAdminCommand(request.Email, request.Password));
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteAdminCommand(id));
        return Ok();
    }
}

