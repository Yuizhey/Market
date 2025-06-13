using Market.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Contact.Queries.GetContactRequests;

public class GetContactRequestsQueryHandler : IRequestHandler<GetContactRequestsQuery, IEnumerable<GetContactRequestsResponse>>
{
    private readonly IContactRequestsRepository _contactRequestsRepository;
    private readonly ILogger<GetContactRequestsQueryHandler> _logger;

    public GetContactRequestsQueryHandler(
        IContactRequestsRepository contactRequestsRepository,
        ILogger<GetContactRequestsQueryHandler> logger)
    {
        _contactRequestsRepository = contactRequestsRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<GetContactRequestsResponse>> Handle(GetContactRequestsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Получение списка контактных запросов. Тип: {Type}", request.Type);
        
        var requests = await _contactRequestsRepository.GetAllAsync(request.Type);
        _logger.LogInformation("Получено {Count} контактных запросов", requests.Count());

        return requests.Select(x => new GetContactRequestsResponse
        {
            Id = x.Id,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Email = x.Email,
            Message = x.Message,
            Type = x.Type,
        });
    }
} 