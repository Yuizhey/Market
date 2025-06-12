using Market.Application.Features.Contact.Commands.AddContactRequest;
using Market.MVC.Models.Contact;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Market.MVC.Controllers;

public class ContactController : Controller
{
    private readonly IMediator _mediator;

    public ContactController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage(ContactVM model)
    {
        if (!ModelState.IsValid)
        {
            return View("/Index", model);
        }

        var command = new AddContactRequestCommand
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            Message = model.Message,
            Type = model.Type
        };

        var result = await _mediator.Send(command);

        if (result.Success)
        {
            TempData["SuccessMessage"] = result.Message;
        }
        else
        {
            TempData["ErrorMessage"] = result.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}