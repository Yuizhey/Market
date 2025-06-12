using Market.Domain.Enums;

namespace Market.Application.Features.Contact.Queries.GetContactRequests;

public class GetContactRequestsResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Message { get; set; } = null!;
    public ContactType Type { get; set; }
} 