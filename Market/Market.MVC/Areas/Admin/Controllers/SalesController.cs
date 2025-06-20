using Market.Application.Features.Sales.Queries.GetAllSales;
using Market.Domain.Enums;
using Market.MVC.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.MVC.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = nameof(UserRoles.Admin))]
public class SalesController : Controller
{
    private readonly IMediator _mediator;

    public SalesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index()
    {
        var sales = await _mediator.Send(new GetAllSalesQuery());
        var vm = new AdminSalesVM { Sales = sales };
        return View(vm);
    }
} 