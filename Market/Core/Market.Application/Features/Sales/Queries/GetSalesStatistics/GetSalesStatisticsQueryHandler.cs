using Market.Application.Interfaces.Repositories;
using MediatR;

namespace Market.Application.Features.Sales.Queries.GetSalesStatistics;

public class GetSalesStatisticsQueryHandler : IRequestHandler<GetSalesStatisticsQuery, IEnumerable<GetSalesStatisticsDto>>
{
    private readonly IProductSaleStatisticsRepository _statisticsRepository;

    public GetSalesStatisticsQueryHandler(IProductSaleStatisticsRepository statisticsRepository)
    {
        _statisticsRepository = statisticsRepository;
    }

    public async Task<IEnumerable<GetSalesStatisticsDto>> Handle(GetSalesStatisticsQuery request, CancellationToken cancellationToken)
    {
        var stats = await _statisticsRepository.GetAllAsync();
        return stats.Select(s => new GetSalesStatisticsDto
        {
            ProductId = s.ProductId,
            ProductTitle = s.Product.Title,
            TotalSalesCount = s.TotalSalesCount,
            TotalRevenue = s.TotalRevenue
        });
    }
} 