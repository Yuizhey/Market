using MediatR;

namespace Market.Application.Features.Auth.Comands.AuthorRegister;

public class AuthorRegisterCommand : IRequest<bool>
{
    public required string AuthorUserName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}