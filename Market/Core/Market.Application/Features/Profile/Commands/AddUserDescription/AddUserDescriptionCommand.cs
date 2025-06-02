using Market.Domain.Enums;
using MediatR;

namespace Market.Application.Features.Profile.Commands.AddUserDescription;

public class AddUserDescriptionCommand : IRequest
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public Gender Gender { get; set; }
    public required string Phone { get; set; }
}