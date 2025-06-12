using Market.Application.Interfaces.Repositories;
using MediatR;

namespace Market.Application.Features.Contact.Commands.DeleteContactRequest;

public class DeleteContactRequestCommandHandler : IRequestHandler<DeleteContactRequestCommand, bool>
{
    private readonly IContactRequestsRepository _contactRequestsRepository;

    public DeleteContactRequestCommandHandler(IContactRequestsRepository contactRequestsRepository)
    {
        _contactRequestsRepository = contactRequestsRepository;
    }

    public async Task<bool> Handle(DeleteContactRequestCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _contactRequestsRepository.DeleteAsync(request.Id);
            return true;
        }
        catch
        {
            return false;
        }
    }
} 