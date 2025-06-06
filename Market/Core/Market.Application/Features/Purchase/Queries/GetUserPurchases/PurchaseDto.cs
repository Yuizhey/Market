namespace Market.Application.Features.Purchase.Queries.GetUserPurchases;

public class PurchaseDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public DateTime PurchaseDate { get; set; }
} 