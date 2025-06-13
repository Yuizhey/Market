using Market.Application.Features.Users.Queries.GetAllUsers;
using Market.Domain.Enums;
using Market.MVC.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Market.MVC.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = nameof(UserRoles.Admin))]
public class UsersController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IMediator mediator, ILogger<UsersController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        _logger.LogInformation("Просмотр списка пользователей администратором");
        
        var users = await _mediator.Send(new GetAllUsersQuery());
        _logger.LogInformation("Загружено {Count} пользователей", users.Count());
        
        var viewModel = new AdminUsersVM { Users = users };
        return View(viewModel);
    }
} 