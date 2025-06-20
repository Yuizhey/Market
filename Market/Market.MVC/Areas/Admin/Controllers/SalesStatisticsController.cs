using Market.Application.Features.Sales.Queries.GetSalesStatistics;
using Market.Domain.Enums;
using Market.MVC.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.MVC.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = nameof(UserRoles.Admin))]
public class SalesStatisticsController : Controller
{
    private readonly IMediator _mediator;

    public SalesStatisticsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index()
    {
        var stats = await _mediator.Send(new GetSalesStatisticsQuery());
        var vm = new AdminSalesStatisticsVM { Statistics = stats };
        return View(vm);
    }
} 