using Market.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Market.Application.Features.Users.Commands.CreateAdmin;

public class CreateAdminCommandHandler : IRequestHandler<CreateAdminCommand>
{
    private readonly UserManager<IdentityUser> _userManager;

    public CreateAdminCommandHandler(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task Handle(CreateAdminCommand request, CancellationToken cancellationToken)
    {
        var user = new IdentityUser
        {
            UserName = request.Email,
            Email = request.Email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, nameof(UserRoles.Admin));
        }
        else
        {
            throw new Exception($"Не удалось создать администратора: {string.Join(", ", result.Errors)}");
        }
    }
} 