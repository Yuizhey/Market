using Market.Application.Interfaces.Repositories;
using MediatR;

namespace Market.Application.Features.Contact.Queries.GetContactRequests;

public class GetContactRequestsQueryHandler : IRequestHandler<GetContactRequestsQuery, IEnumerable<GetContactRequestsResponse>>
{
    private readonly IContactRequestsRepository _contactRequestsRepository;

    public GetContactRequestsQueryHandler(IContactRequestsRepository contactRequestsRepository)
    {
        _contactRequestsRepository = contactRequestsRepository;
    }

    public async Task<IEnumerable<GetContactRequestsResponse>> Handle(GetContactRequestsQuery request, CancellationToken cancellationToken)
    {
        var requests = await _contactRequestsRepository.GetAllAsync(request.Type);

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