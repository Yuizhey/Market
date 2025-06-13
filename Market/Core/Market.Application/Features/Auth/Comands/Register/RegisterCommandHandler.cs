using Market.Application.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Auth.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, bool>
{
    private readonly IAuthService _authSerivice;
    private readonly ILogger<RegisterCommandHandler> _logger;
    
    public RegisterCommandHandler(
        IAuthService authSerivice,
        ILogger<RegisterCommandHandler> logger)
    {
        _authSerivice = authSerivice;
        _logger = logger;
    }
    
    public async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Попытка регистрации нового пользователя с email: {Email}", request.Email);
        var result = await _authSerivice.RegisterAsync(request.FullName, request.Password, request.Email, request.ConfirmPassword);
        
        if (result)
        {
            _logger.LogInformation("Успешная регистрация пользователя с email: {Email}", request.Email);
        }
        else
        {
            _logger.LogWarning("Неудачная попытка регистрации пользователя с email: {Email}", request.Email);
        }
        
        return result;
    }
}