using MediatR;

namespace Market.Application.Features.Products.Queries.GetSalesStatistics;

public record GetSalesStatisticsQuery : IRequest<IEnumerable<GetSalesStatisticsDto>>; 