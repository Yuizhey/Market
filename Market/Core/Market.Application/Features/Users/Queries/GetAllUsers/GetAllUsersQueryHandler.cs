using Market.Application.Interfaces.Repositories;
using MediatR;

namespace Market.Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<GetAllUsersResponse>>
{
    private readonly IUserDescriptionRepository _userDescriptionRepository;

    public GetAllUsersQueryHandler(IUserDescriptionRepository userDescriptionRepository)
    {
        _userDescriptionRepository = userDescriptionRepository;
    }

    public async Task<IEnumerable<GetAllUsersResponse>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userDescriptionRepository.GetAllAsync();
        
        return users.Select(u => new GetAllUsersResponse
        {
            Id = u.Id,
            IdentityUserId = u.IdentityUserId,
            Email = u.Email,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Gender = u.Gender,
            Phone = u.Phone,
        });
    }
} 