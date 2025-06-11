using System.Security.Claims;
using Market.Application.Features.Likes.Commands;
using Market.Application.Interfaces.Repositories;
using Market.Domain.Entities;
using Market.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

public class ToggleLikeCommandHandler : IRequestHandler<ToggleLikeCommand>
{
    private readonly ILikeRepository _likeRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserDescriptionRepository _userDescriptionRepository;
    private readonly IAuthorUserDescriptionRepository _authorUserDescriptionRepository;

    public ToggleLikeCommandHandler(
        ILikeRepository likeRepository,
        IHttpContextAccessor httpContextAccessor,
        IUserDescriptionRepository userDescriptionRepository,
        IAuthorUserDescriptionRepository authorUserDescriptionRepository)
    {
        _likeRepository = likeRepository;
        _httpContextAccessor = httpContextAccessor;
        _userDescriptionRepository = userDescriptionRepository;
        _authorUserDescriptionRepository = authorUserDescriptionRepository;
    }

    public async Task Handle(ToggleLikeCommand request, CancellationToken cancellationToken)
    {
        var identityUserId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);

        Guid userId;
        if (userRole == UserRoles.CLientUser.ToString())
        {
            var businessId = await _userDescriptionRepository.GetBusinessIdByIdentityUserIdAsync(Guid.Parse(identityUserId!));
            if (businessId == null)
                throw new InvalidOperationException("User description not found");
            userId = businessId;
        }
        else if (userRole == UserRoles.AuthorUser.ToString())
        {
            var businessId = await _authorUserDescriptionRepository.GetBusinessIdByIdentityUserIdAsync(Guid.Parse(identityUserId!));
            if (businessId == null)
                throw new InvalidOperationException("Author user description not found");
            userId = businessId;
        }
        else
        {
            throw new UnauthorizedAccessException();
        }

        var existingLike = await _likeRepository.GetByUserAndProductAsync(userId, request.ProductId);

        if (existingLike is not null)
        {
            await _likeRepository.DeleteAsync(existingLike);
        }
        else
        {
            var newLike = new Like
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ProductId = request.ProductId
            };
            await _likeRepository.AddAsync(newLike);
        }
    }
}
