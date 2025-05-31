using Market.Application.Interfaces.Services;
using MediatR;

namespace Market.Application.Features.Auth.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, bool>
{
    private readonly IAuthService _authService;

    public LoginQueryHandler(IAuthService authService)
    {
        _authService = authService;
    }
    
    public async Task<bool> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        return await _authService.LoginAsync(request.Email, request.Password);
    }
}