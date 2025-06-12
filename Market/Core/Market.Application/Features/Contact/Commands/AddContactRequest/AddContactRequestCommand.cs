using Market.Domain.Enums;
using MediatR;

namespace Market.Application.Features.Contact.Commands.AddContactRequest;

public class AddContactRequestCommand : IRequest<AddContactRequestResponse>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Message { get; set; }
    public ContactType Type { get; set; }
} 