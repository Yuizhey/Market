using Market.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Market.Application.Features.Users.Commands.DeleteAdmin;

public class DeleteAdminCommandHandler : IRequestHandler<DeleteAdminCommand>
{
    private readonly UserManager<IdentityUser> _userManager;

    public DeleteAdminCommandHandler(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task Handle(DeleteAdminCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        if (user == null)
        {
            throw new Exception("Администратор не найден");
        }

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            throw new Exception($"Не удалось удалить администратора: {string.Join(", ", result.Errors)}");
        }
    }
} 