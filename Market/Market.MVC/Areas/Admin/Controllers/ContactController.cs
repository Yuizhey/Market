using Market.Application.Features.Contact.Queries.GetContactRequests;
using Market.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.MVC.Areas.Admin.Controllers;

[Area("Admin")]
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
        return View(requests);
    }
}