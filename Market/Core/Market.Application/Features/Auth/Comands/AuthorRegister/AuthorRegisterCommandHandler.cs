using Market.Application.Features.Auth.Register;
using Market.Application.Interfaces.Services;
using MediatR;

namespace Market.Application.Features.Auth.Comands.AuthorRegister;

public class AuthorRegisterCommandHandler : IRequestHandler<AuthorRegisterCommand, bool>
{
    private readonly IAuthService _authSerivice;
    
    public AuthorRegisterCommandHandler(IAuthService authSerivice)
    {
        _authSerivice = authSerivice;
    }
    
    public async Task<bool> Handle(AuthorRegisterCommand request, CancellationToken cancellationToken)
    {
        return await _authSerivice.AuthorRegisterAsync(request.AuthorUserName, request.Password, request.Email);
    }
}