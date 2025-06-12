using System.Security.Claims;
using Market.Application.Interfaces.Repositories;
using Market.Application.Interfaces.Services;
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
        var products = await _productRepository.GetFilteredProductsAsync(
            request.Page,
            request.PageSize,
            request.ProductTypes,
            request.MinPrice,
            request.MaxPrice);

        var totalItems = await _productRepository.GetFilteredProductsCountAsync(
            request.ProductTypes,
            request.MinPrice,
            request.MaxPrice);

        var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

        var productDtos = products.Select(p => new GetByPageNumberDto
        {
            Id = p.Id,
            Title = p.Title,
            Price = p.Price,
            ImageURL = p.CoverImagePath,
            IsLiked = false // Это значение будет установлено позже
        });

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

        var productDtosList = new List<GetByPageNumberDto>();
        foreach (var product in productDtos)
        {
            string? imageUrl = null;
            if (!string.IsNullOrEmpty(product.ImageURL))
            {
                try
                {
                    imageUrl = await _minioService.GetCoverImageUrlAsync(product.ImageURL, cancellationToken);
                }
                catch
                {
                    imageUrl = "assets/img/520x400.png";
                }
            }

            productDtosList.Add(new GetByPageNumberDto
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
            Products = productDtosList,
            TotalPages = totalPages,
            CurrentPage = request.Page
        };
    }
}
