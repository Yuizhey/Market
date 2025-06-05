using System.Security.Claims;
using Market.Application.Interfaces.Repositories;
using Market.Application.Interfaces.Services;
using Market.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Market.Application.Features.Products.Commands;

public class AddNewProductCommandHandler : IRequestHandler<AddNewProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuthorUserDescriptionRepository _authorUserDescriptionRepository;
    private readonly IMinioService _minioService;

    public AddNewProductCommandHandler(
        IProductRepository productRepository,
        IHttpContextAccessor httpContextAccessor,
        IAuthorUserDescriptionRepository authorUserDescriptionRepository,
        IMinioService minioService)
    {
        _productRepository = productRepository;
        _httpContextAccessor = httpContextAccessor;
        _authorUserDescriptionRepository = authorUserDescriptionRepository;
        _minioService = minioService;
    }

    public async Task Handle(AddNewProductCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var authorUserId))
        {
            throw new UnauthorizedAccessException("Пользователь не аутентифицирован.");
        }

        var authorId = await _authorUserDescriptionRepository.GetBusinessIdByIdentityUserIdAsync(authorUserId);
        var product = new Product
        {
            Title = request.Title,
            Text = request.Text,
            Price = request.Price,
            Id = Guid.NewGuid(),
            AuthorUserId = authorId
        };
        
        if (request.CoverImage != null)
        {
            product.CoverImagePath = await _minioService.UploadCoverImageAsync(request.CoverImage, product.Id, cancellationToken);
        }

        await _productRepository.AddProductAsync(product);
    }
}