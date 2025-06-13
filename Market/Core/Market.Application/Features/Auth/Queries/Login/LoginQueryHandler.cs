using Market.Application.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Auth.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, bool>
{
    private readonly IAuthService _authService;
    private readonly ILogger<LoginQueryHandler> _logger;

    public LoginQueryHandler(
        IAuthService authService,
        ILogger<LoginQueryHandler> logger)
    {
        _authService = authService;
        _logger = logger;
    }
    
    public async Task<bool> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Попытка входа пользователя с email: {Email}", request.Email);
        var result = await _authService.LoginAsync(request.Email, request.Password);
        
        if (result)
        {
            _logger.LogInformation("Успешный вход пользователя с email: {Email}", request.Email);
        }
        else
        {
            _logger.LogWarning("Неудачная попытка входа пользователя с email: {Email}", request.Email);
        }
        
        return result;
    }
}