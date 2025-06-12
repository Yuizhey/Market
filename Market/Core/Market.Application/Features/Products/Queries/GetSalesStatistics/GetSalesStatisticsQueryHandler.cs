using Market.Application.Interfaces.Repositories;
using Market.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Market.Application.Features.Products.Queries.GetSalesStatistics;

public class GetSalesStatisticsQueryHandler : IRequestHandler<GetSalesStatisticsQuery, IEnumerable<GetSalesStatisticsDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductSaleStatisticsRepository _statisticsRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetSalesStatisticsQueryHandler(
        IProductRepository productRepository,
        IProductSaleStatisticsRepository statisticsRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _productRepository = productRepository;
        _statisticsRepository = statisticsRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<GetSalesStatisticsDto>> Handle(GetSalesStatisticsQuery request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("Пользователь не авторизован");
        }

        var authorId = Guid.Parse(userId);
        var products = await _productRepository.GetProductsByUserId(authorId);
        var productIds = products.Select(p => p.Id).ToList();
        
        var statistics = await _statisticsRepository.GetByProductIdsAsync(productIds);
        var statisticsDict = statistics.ToDictionary(s => s.ProductId);

        return products.Select(p => new GetSalesStatisticsDto
        {
            ProductId = p.Id,
            ProductTitle = p.Title,
            TotalSalesCount = statisticsDict.TryGetValue(p.Id, out var stats) ? stats.TotalSalesCount : 0,
            TotalRevenue = statisticsDict.TryGetValue(p.Id, out stats) ? stats.TotalRevenue : 0,
        });
    }
} 