using Market.Domain.Enums;
using MediatR;

namespace Market.Application.Features.Contact.Queries.GetContactRequests;

public class GetContactRequestsQuery : IRequest<IEnumerable<GetContactRequestsResponse>>
{
    public ContactType? Type { get; set; }
} 