using Market.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Market.Application.Features.Users.Queries.GetAdmins;

public class GetAdminsQueryHandler : IRequestHandler<GetAdminsQuery, IEnumerable<AdminDto>>
{
    private readonly UserManager<IdentityUser> _userManager;

    public GetAdminsQueryHandler(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IEnumerable<AdminDto>> Handle(GetAdminsQuery request, CancellationToken cancellationToken)
    {
        var adminRole = nameof(UserRoles.Admin);
        var admins = await _userManager.GetUsersInRoleAsync(adminRole);
        
        return admins.Select(admin => new AdminDto
        {
            Id = Guid.Parse(admin.Id),
            Email = admin.Email,
        });
    }
} 