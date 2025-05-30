using Market.Application.Interfaces.Services;
using MediatR;

namespace Market.Application.Features.Auth.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, bool>
{
    private readonly IAuthService _authSerivice;
    
    public RegisterCommandHandler(IAuthService authSerivice)
    {
        _authSerivice = authSerivice;
    }
    
    public async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
       return await _authSerivice.RegisterAsync(request.FullName, request.Password, request.Email, request.ConfirmPassword);
    }
}