namespace Market.Application.Features.Products.Queries.GetSalesStatistics;

public class GetSalesStatisticsDto
{
    public Guid ProductId { get; set; }
    public string ProductTitle { get; set; } = string.Empty;
    public int TotalSalesCount { get; set; }
    public decimal TotalRevenue { get; set; }
} 