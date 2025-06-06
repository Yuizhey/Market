using Market.Application.Interfaces.Repositories;
using Market.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Market.Domain.Enums;

namespace Market.Application.Features.Purchase.Queries.GetUserPurchases;

public class GetUserPurchasesQueryHandler : IRequestHandler<GetUserPurchasesQuery, IEnumerable<PurchaseDto>>
{
    private readonly IPurchaseRepository _purchaseRepository;
    private readonly IUserDescriptionRepository _userDescriptionRepository;
    private readonly IAuthorUserDescriptionRepository _authorUserDescriptionRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetUserPurchasesQueryHandler(
        IPurchaseRepository purchaseRepository,
        IUserDescriptionRepository userDescriptionRepository,
        IAuthorUserDescriptionRepository authorUserDescriptionRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _purchaseRepository = purchaseRepository;
        _userDescriptionRepository = userDescriptionRepository;
        _authorUserDescriptionRepository = authorUserDescriptionRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<PurchaseDto>> Handle(GetUserPurchasesQuery request, CancellationToken cancellationToken)
    {
        var identityUserId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        
        Guid userId;
        if (userRole == UserRoles.CLientUser.ToString())
        {
            var userDescriptionId = await _userDescriptionRepository.GetBusinessIdByIdentityUserIdAsync(Guid.Parse(identityUserId!));
            if (userDescriptionId == null)
                throw new InvalidOperationException("User description not found");
            userId = userDescriptionId;
        }
        else if (userRole == UserRoles.AuthorUser.ToString())
        {
            var authorUserDescriptionId = await _authorUserDescriptionRepository.GetBusinessIdByIdentityUserIdAsync(Guid.Parse(identityUserId!));
            if (authorUserDescriptionId == null)
                throw new InvalidOperationException("Author user description not found");
            userId = authorUserDescriptionId;
        }
        else
        {
            throw new InvalidOperationException("Invalid user role");
        }
        
        var purchases = await _purchaseRepository.GetPurchasesByBuyerIdAsync(userId);

        return purchases.Select(p => new PurchaseDto
        {
            ProductId = p.ProductId,
            ProductName = p.Product.Title,
            Price = p.Price,
            PurchaseDate = p.PurchaseDate,
        });
    }
} 