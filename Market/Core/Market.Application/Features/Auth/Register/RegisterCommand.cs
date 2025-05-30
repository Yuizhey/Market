using MediatR;
using Microsoft.Win32;

namespace Market.Application.Features.Auth.Register;

public class RegisterCommand : IRequest<bool>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string ConfirmPassword { get; init; }
    public required string FullName { get; init; }
}