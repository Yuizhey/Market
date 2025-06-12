using System.Security.Claims;
using Market.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Market.Application.Features.Profile.Queries.GetUserProfile;

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, GetUserProfileResponse>
{
    private readonly IUserDescriptionRepository _userDescriptionRepository;
    private readonly IAuthorUserDescriptionRepository _authorUserDescriptionRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetUserProfileQueryHandler(
        IUserDescriptionRepository userDescriptionRepository,
        IAuthorUserDescriptionRepository authorUserDescriptionRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _userDescriptionRepository = userDescriptionRepository;
        _authorUserDescriptionRepository = authorUserDescriptionRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<GetUserProfileResponse> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var role = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role);

        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("User is not authenticated");

        var identityUserId = Guid.Parse(userId);

        if (role == "Author")
        {
            var authorProfile = await _authorUserDescriptionRepository.GetByIdentityUserIdAsync(identityUserId);
            if (authorProfile == null)
                return new GetUserProfileResponse();

            return new GetUserProfileResponse
            {
                FirstName = authorProfile.FirstName,
                LastName = authorProfile.LastName,
                Gender = authorProfile.Gender,
                Phone = authorProfile.Phone,
                Email = authorProfile.Email,
                Country = authorProfile.Country,
                HomeAddress = authorProfile.HomeAddress,
                Address = authorProfile.Address
            };
        }
        else
        {
            var userProfile = await _userDescriptionRepository.GetByIdentityUserIdAsync(identityUserId);
            if (userProfile == null)
                return new GetUserProfileResponse();

            return new GetUserProfileResponse
            {
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                Gender = userProfile.Gender,
                Phone = userProfile.Phone,
                Email = userProfile.Email
            };
        }
    }
} 