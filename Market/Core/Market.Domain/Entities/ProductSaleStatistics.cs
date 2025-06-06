using Market.Domain.Common;

namespace Market.Domain.Entities;

public sealed class ProductSaleStatistics : BaseAuditableEntity
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }
    public Product Product { get; set; }

    public int TotalSalesCount { get; set; }
    public decimal TotalRevenue { get; set; }
}