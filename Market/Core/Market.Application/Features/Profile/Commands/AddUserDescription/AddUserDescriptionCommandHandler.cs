using MediatR;

namespace Market.Application.Features.Profile.Commands.AddUserDescription;

public class AddUserDescriptionCommandHandler : IRequestHandler<AddUserDescriptionCommand>
{
    public Task Handle(AddUserDescriptionCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}