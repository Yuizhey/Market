using MediatR;

namespace Market.Application.Features.Auth.Queries.Login;

public class LoginQuery : IRequest<bool>
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}