using Market.Domain.Enums;
using MediatR;

namespace Market.Application.Features.Profile.Commands.AddAuthorUserDescription;

public class AddAuthorUserDescriptionCommand : IRequest
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Country { get; set; }
    public Gender Gender { get; set; }
    public required string Phone { get; set; }
    public required string HomeAddress { get; set; }
    public required string Address { get; set; }
}