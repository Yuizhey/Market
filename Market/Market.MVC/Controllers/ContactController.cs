using Market.Application.Features.Contact.Commands.AddContactRequest;
using Market.MVC.Models.Contact;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Market.MVC.Controllers;

public class ContactController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<ContactController> _logger;

    public ContactController(IMediator mediator, ILogger<ContactController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public IActionResult Index()
    {
        _logger.LogInformation("Просмотр страницы контактов");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage(ContactVM model)
    {
        _logger.LogInformation("Отправка сообщения от {FirstName} {LastName} ({Email})", 
            model.FirstName, model.LastName, model.Email);

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Невалидная форма контакта от {Email}", model.Email);
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
            _logger.LogInformation("Сообщение успешно отправлено от {Email}", model.Email);
            TempData["SuccessMessage"] = result.Message;
        }
        else
        {
            _logger.LogWarning("Ошибка при отправке сообщения от {Email}: {Message}", 
                model.Email, result.Message);
            TempData["ErrorMessage"] = result.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}