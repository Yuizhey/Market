using MediatR;

namespace Market.Application.Features.Contact.Commands.DeleteContactRequest;

public class DeleteContactRequestCommand : IRequest<bool>
{
    public Guid Id { get; set; }
} 