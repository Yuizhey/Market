using Market.Application.Features.Users.Queries.GetAllUsers;
using Market.Domain.Enums;
using Market.MVC.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.MVC.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = nameof(UserRoles.Admin))]
public class UsersController : Controller
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _mediator.Send(new GetAllUsersQuery());
        var viewModel = new AdminUsersVM { Users = users };
        return View(viewModel);
    }
} 