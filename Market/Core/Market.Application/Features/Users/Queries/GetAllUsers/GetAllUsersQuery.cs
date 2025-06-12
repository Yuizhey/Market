using MediatR;

namespace Market.Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQuery : IRequest<IEnumerable<GetAllUsersResponse>>
{
} 