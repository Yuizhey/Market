using Market.Application.Interfaces.Services;
using MediatR;

namespace Market.Application.Features.Auth.Comands.Logout;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand>
{
    private readonly IAuthService _authService;

    public LogoutCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await _authService.LogoutAsync();
    }
}