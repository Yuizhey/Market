using Market.Domain.Enums;

namespace Market.Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersResponse
{
    public Guid Id { get; set; }
    public Guid IdentityUserId { get; set; }
    public string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Gender? Gender { get; set; }
    public string? Phone { get; set; }
} 