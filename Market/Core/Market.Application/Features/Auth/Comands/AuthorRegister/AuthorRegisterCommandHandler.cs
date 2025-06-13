using Market.Application.Features.Auth.Register;
using Market.Application.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Auth.Comands.AuthorRegister;

public class AuthorRegisterCommandHandler : IRequestHandler<AuthorRegisterCommand, bool>
{
    private readonly IAuthService _authSerivice;
    private readonly ILogger<AuthorRegisterCommandHandler> _logger;
    
    public AuthorRegisterCommandHandler(
        IAuthService authSerivice,
        ILogger<AuthorRegisterCommandHandler> logger)
    {
        _authSerivice = authSerivice;
        _logger = logger;
    }
    
    public async Task<bool> Handle(AuthorRegisterCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Попытка регистрации нового автора с email: {Email}", request.Email);
        var result = await _authSerivice.AuthorRegisterAsync(request.AuthorUserName, request.Password, request.Email);
        
        if (result)
        {
            _logger.LogInformation("Успешная регистрация автора с email: {Email}", request.Email);
        }
        else
        {
            _logger.LogWarning("Неудачная попытка регистрации автора с email: {Email}", request.Email);
        }
        
        return result;
    }
}