using Market.Application.Interfaces.Repositories;
using Market.Domain.Entities;
using MediatR;

namespace Market.Application.Features.Contact.Commands.AddContactRequest;

public class AddContactRequestCommandHandler : IRequestHandler<AddContactRequestCommand, AddContactRequestResponse>
{
    private readonly IContactRequestsRepository _contactRequestsRepository;

    public AddContactRequestCommandHandler(IContactRequestsRepository contactRequestsRepository)
    {
        _contactRequestsRepository = contactRequestsRepository;
    }

    public async Task<AddContactRequestResponse> Handle(AddContactRequestCommand request, CancellationToken cancellationToken)
    {
        try
        {
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

            return new AddContactRequestResponse
            {
                Success = true,
                Message = "Your message has been sent successfully. We will contact you soon."
            };
        }
        catch (Exception ex)
        {
            return new AddContactRequestResponse
            {
                Success = false,
                Message = "An error occurred while sending your message. Please try again later."
            };
        }
    }
} 