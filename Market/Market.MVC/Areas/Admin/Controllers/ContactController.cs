using Market.Application.Features.Contact.Commands.DeleteContactRequest;
using Market.Application.Features.Contact.Queries.GetContactRequests;
using Market.Domain.Enums;
using Market.MVC.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Market.MVC.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = nameof(UserRoles.Admin))]
public class ContactController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<ContactController> _logger;

    public ContactController(IMediator mediator, ILogger<ContactController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<IActionResult> Index(ContactType? type = null)
    {
        _logger.LogInformation("Просмотр списка контактных запросов. Тип: {Type}", type);
        
        var query = new GetContactRequestsQuery { Type = type };
        var requests = await _mediator.Send(query);
        _logger.LogInformation("Загружено {Count} контактных запросов", requests.Count());
        
        var viewModel = new AdminContactVM()
        {
            Responses = requests
        };
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Delete([FromBody] DeleteContactRequestCommand command)
    {
        _logger.LogInformation("Попытка удаления контактного запроса");
        
        var result = await _mediator.Send(command);
        if (result)
        {
            _logger.LogInformation("Контактный запрос успешно удален");
        }
        else
        {
            _logger.LogWarning("Не удалось удалить контактный запрос");
        }
        
        return Json(new { success = result });
    }
}