using MediatR;
using System.Collections.Generic;

namespace Market.Application.Features.Sales.Queries.GetAllSales;

public record GetAllSalesQuery : IRequest<IEnumerable<GetAllSalesDto>>; 