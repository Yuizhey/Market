using Market.Application.Features.Authors.Queries.GetAllAuthors;
using Market.Domain.Enums;
using Market.MVC.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Market.MVC.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = nameof(UserRoles.Admin))]
public class AuthorsController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthorsController> _logger;

    public AuthorsController(IMediator mediator, ILogger<AuthorsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        _logger.LogInformation("Просмотр списка авторов администратором");
        var authors = await _mediator.Send(new GetAllAuthorsQuery());
        _logger.LogInformation("Загружено {Count} авторов", authors.Count());
        
        var viewModel = new AdminAuthorsVM { Authors = authors };
        return View(viewModel);
    }
} 