using Market.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Contact.Commands.DeleteContactRequest;

public class DeleteContactRequestCommandHandler : IRequestHandler<DeleteContactRequestCommand, bool>
{
    private readonly IContactRequestsRepository _contactRequestsRepository;
    private readonly ILogger<DeleteContactRequestCommandHandler> _logger;

    public DeleteContactRequestCommandHandler(
        IContactRequestsRepository contactRequestsRepository,
        ILogger<DeleteContactRequestCommandHandler> logger)
    {
        _contactRequestsRepository = contactRequestsRepository;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteContactRequestCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Попытка удаления контактного запроса с ID: {Id}", request.Id);
            await _contactRequestsRepository.DeleteAsync(request.Id);
            _logger.LogInformation("Контактный запрос успешно удален с ID: {Id}", request.Id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при удалении контактного запроса с ID: {Id}", request.Id);
            return false;
        }
    }
} 