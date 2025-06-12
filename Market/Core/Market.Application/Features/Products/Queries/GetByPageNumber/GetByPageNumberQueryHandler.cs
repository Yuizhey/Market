using System.Security.Claims;
using Market.Application.Interfaces.Repositories;
using Market.Application.Interfaces.Services;
using Market.Domain.Entities;
using Market.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Market.Application.Features.Products.Queries.GetByPageNumber;

public class GetByPageNumberQueryHandler : IRequestHandler<GetByPageNumberQuery, GetByPageNumberREsult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMinioService _minioService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserDescriptionRepository _userDescriptionRepository;
    private readonly IAuthorUserDescriptionRepository _authorUserDescriptionRepository;
    private readonly ILikeRepository _likeRepository;

    public GetByPageNumberQueryHandler(
        IProductRepository productRepository,
        IMinioService minioService,
        IHttpContextAccessor httpContextAccessor,
        IUserDescriptionRepository userDescriptionRepository,
        IAuthorUserDescriptionRepository authorUserDescriptionRepository,
        ILikeRepository likeRepository)
    {
        _productRepository = productRepository;
        _minioService = minioService;
        _httpContextAccessor = httpContextAccessor;
        _userDescriptionRepository = userDescriptionRepository;
        _authorUserDescriptionRepository = authorUserDescriptionRepository;
        _likeRepository = likeRepository;
    }

    public async Task<GetByPageNumberREsult> Handle(GetByPageNumberQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Product> products;
        int totalCount;

        if (request.Types != null && request.Types.Any())
        {
            products = await _productRepository.GetProductsByTypes(request.Types, request.Page, request.PageSize);
            totalCount = await _productRepository.GetTotalProductCountByTypesAsync(request.Types);
        }
        else
        {
            products = await _productRepository.GetProductsByPage(request.Page, request.PageSize);
            totalCount = await _productRepository.GetTotalProductCountAsync();
        }

        var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);
        
        var identityUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);

        HashSet<Guid> usersLikes = new();

        if (!string.IsNullOrEmpty(identityUserId) && !string.IsNullOrEmpty(userRole))
        {
            try
            {
                Guid userId;
                if (userRole == UserRoles.CLientUser.ToString())
                {
                    var userDescriptionId = await _userDescriptionRepository.GetBusinessIdByIdentityUserIdAsync(Guid.Parse(identityUserId));
                    if (userDescriptionId != null)
                        userId = userDescriptionId;
                    else
                        throw new InvalidOperationException("User description not found");

                    usersLikes = (await _likeRepository.GetLikedProductIdsByUserIdAsync(userId)).ToHashSet();
                }
                else if (userRole == UserRoles.AuthorUser.ToString())
                {
                    var authorUserDescriptionId = await _authorUserDescriptionRepository.GetBusinessIdByIdentityUserIdAsync(Guid.Parse(identityUserId));
                    if (authorUserDescriptionId != null)
                        userId = authorUserDescriptionId;
                    else
                        throw new InvalidOperationException("Author user description not found");

                    usersLikes = (await _likeRepository.GetLikedProductIdsByUserIdAsync(userId)).ToHashSet();
                }
            }
            catch
            {
            }
        }

        var productDtos = new List<GetByPageNumberDto>();
        foreach (var product in products)
        {
            string? imageUrl = null;
            if (!string.IsNullOrEmpty(product.CoverImagePath))
            {
                try
                {
                    imageUrl = await _minioService.GetCoverImageUrlAsync(product.CoverImagePath, cancellationToken);
                }
                catch
                {
                    imageUrl = "assets/img/520x400.png";
                }
            }

            productDtos.Add(new GetByPageNumberDto
            {
                Id = product.Id,
                Title = product.Title,
                Price = product.Price,
                ImageURL = imageUrl ?? "assets/img/520x400.png",
                IsLiked = usersLikes.Contains(product.Id)
            });
        }

        return new GetByPageNumberREsult
        {
            Products = productDtos,
            TotalPages = totalPages,
            CurrentPage = request.Page
        };
    }

}
