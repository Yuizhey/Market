using System;

namespace Market.Application.Features.Sales.Queries.GetSalesStatistics;

public class GetSalesStatisticsDto
{
    public Guid ProductId { get; set; }
    public string ProductTitle { get; set; }
    public int TotalSalesCount { get; set; }
    public decimal TotalRevenue { get; set; }
} 