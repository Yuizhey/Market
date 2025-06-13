using System.Security.Claims;
using Market.Application.Interfaces.Repositories;
using Market.Application.Interfaces.Services;
using Market.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Products.Commands;

public class AddNewProductCommandHandler : IRequestHandler<AddNewProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuthorUserDescriptionRepository _authorUserDescriptionRepository;
    private readonly IMinioService _minioService;
    private readonly ILogger<AddNewProductCommandHandler> _logger;

    public AddNewProductCommandHandler(
        IProductRepository productRepository,
        IHttpContextAccessor httpContextAccessor,
        IAuthorUserDescriptionRepository authorUserDescriptionRepository,
        IMinioService minioService,
        ILogger<AddNewProductCommandHandler> logger)
    {
        _productRepository = productRepository;
        _httpContextAccessor = httpContextAccessor;
        _authorUserDescriptionRepository = authorUserDescriptionRepository;
        _minioService = minioService;
        _logger = logger;
    }

    public async Task Handle(AddNewProductCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var authorUserId))
        {
            _logger.LogError("Попытка создания товара неаутентифицированным пользователем");
            throw new UnauthorizedAccessException("Пользователь не аутентифицирован.");
        }

        _logger.LogInformation("Начало создания нового товара автором {AuthorId}", authorUserId);

        var authorId = await _authorUserDescriptionRepository.GetBusinessIdByIdentityUserIdAsync(authorUserId);
        if (authorId == null)
        {
            _logger.LogError("Не найден профиль автора для {AuthorId}", authorUserId);
            throw new InvalidOperationException("Author profile not found");
        }

        var product = new Product
        {
            Title = request.Title,
            Text = request.Text,
            Price = request.Price,
            Id = Guid.NewGuid(),
            AuthorUserId = authorId,
            Subtitle = request.Subtitle,
            ShortDescription = request.ShortDescription,
            ProductType = request.ProductType
        };

        _logger.LogInformation("Создан новый товар {ProductId} типа {ProductType}", product.Id, product.ProductType);

        if (request.CoverImage != null)
        {
            _logger.LogInformation("Загрузка обложки для товара {ProductId}", product.Id);
            product.CoverImagePath = await _minioService.UploadCoverImageAsync(request.CoverImage, product.Id, cancellationToken);
            _logger.LogInformation("Обложка успешно загружена для товара {ProductId}", product.Id);
        }

        if (request.AdditionalFiles != null && request.AdditionalFiles.Any())
        {
            _logger.LogInformation("Загрузка дополнительных файлов для товара {ProductId}. Количество файлов: {FileCount}", 
                product.Id, request.AdditionalFiles.Count());
            var filePaths = await _minioService.UploadAdditionalFilesAsync(request.AdditionalFiles, product.Id, cancellationToken);
            product.AdditionalFilePaths = filePaths;
            _logger.LogInformation("Дополнительные файлы успешно загружены для товара {ProductId}", product.Id);
        }

        await _productRepository.AddProductAsync(product);
        _logger.LogInformation("Товар {ProductId} успешно создан и сохранен в базе данных", product.Id);
    }
}