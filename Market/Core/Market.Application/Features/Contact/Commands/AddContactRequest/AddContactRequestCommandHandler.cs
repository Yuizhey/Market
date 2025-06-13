using Market.Application.Interfaces.Repositories;
using Market.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Contact.Commands.AddContactRequest;

public class AddContactRequestCommandHandler : IRequestHandler<AddContactRequestCommand, AddContactRequestResponse>
{
    private readonly IContactRequestsRepository _contactRequestsRepository;
    private readonly ILogger<AddContactRequestCommandHandler> _logger;

    public AddContactRequestCommandHandler(
        IContactRequestsRepository contactRequestsRepository,
        ILogger<AddContactRequestCommandHandler> logger)
    {
        _contactRequestsRepository = contactRequestsRepository;
        _logger = logger;
    }

    public async Task<AddContactRequestResponse> Handle(AddContactRequestCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Создание нового контактного запроса от {FirstName} {LastName} ({Email})", 
                request.FirstName, request.LastName, request.Email);

            var entity = new ContactRequests
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Message = request.Message,
                Type = request.Type
            };

            await _contactRequestsRepository.AddAsync(entity);
            _logger.LogInformation("Контактный запрос успешно создан с ID: {Id}", entity.Id);

            return new AddContactRequestResponse
            {
                Success = true,
                Message = "Your message has been sent successfully. We will contact you soon."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при создании контактного запроса от {Email}", request.Email);
            return new AddContactRequestResponse
            {
                Success = false,
                Message = "An error occurred while sending your message. Please try again later."
            };
        }
    }
} 