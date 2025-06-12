using Market.Application.Features.Authors.Queries.GetAllAuthors;
using Market.MVC.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.MVC.Areas.Admin.Controllers;

[Area("Admin")]
public class AuthorsController : Controller
{
    private readonly IMediator _mediator;

    public AuthorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index()
    {
        var authors = await _mediator.Send(new GetAllAuthorsQuery());
        var viewModel = new AdminAuthorsVM { Authors = authors };
        return View(viewModel);
    }
} 