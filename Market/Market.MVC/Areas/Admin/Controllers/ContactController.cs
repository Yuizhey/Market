using Market.Application.Features.Contact.Commands.DeleteContactRequest;
using Market.Application.Features.Contact.Queries.GetContactRequests;
using Market.Domain.Enums;
using Market.MVC.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.MVC.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = nameof(UserRoles.Admin))]
public class ContactController : Controller
{
    private readonly IMediator _mediator;

    public ContactController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index(ContactType? type = null)
    {
        var query = new GetContactRequestsQuery { Type = type };
        var requests = await _mediator.Send(query);
        var viewModel = new AdminContactVM()
        {
            Responses = requests
        };
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Delete([FromBody] DeleteContactRequestCommand command)
    {
        var result = await _mediator.Send(command);
        return Json(new { success = result });
    }
}