using MediatR;
using System.Collections.Generic;

namespace Market.Application.Features.Sales.Queries.GetSalesStatistics;

public record GetSalesStatisticsQuery : IRequest<IEnumerable<GetSalesStatisticsDto>>; 