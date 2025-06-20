using System;

namespace Market.Application.Features.Sales.Queries.GetAllSales;

public class GetAllSalesDto
{
    public Guid Id { get; set; }
    public string ProductTitle { get; set; }
    public string BuyerId { get; set; }
    public string SellerName { get; set; }
    public decimal Price { get; set; }
    public DateTime PurchaseDate { get; set; }
    public int Quantity { get; set; }
} 