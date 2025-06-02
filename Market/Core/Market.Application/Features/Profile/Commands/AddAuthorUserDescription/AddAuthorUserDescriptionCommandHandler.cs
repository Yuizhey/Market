using MediatR;

namespace Market.Application.Features.Profile.Commands.AddAuthorUserDescription;

public class AddAuthorUserDescriptionCommandHandler : IRequestHandler<AddAuthorUserDescriptionCommand>
{
    public Task Handle(AddAuthorUserDescriptionCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}