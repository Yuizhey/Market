using Market.Domain.Enums;

namespace Market.Application.Features.Profile.Queries.GetUserProfile;

public class GetUserProfileResponse
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Gender? Gender { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Country { get; set; }
    public string? HomeAddress { get; set; }
    public string? Address { get; set; }
} 