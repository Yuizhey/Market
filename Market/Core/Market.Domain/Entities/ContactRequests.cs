using Market.Domain.Enums;

namespace Market.Domain.Entities;

public class ContactRequests
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Message { get; set; }
    public ContactType Type { get; set; }
}