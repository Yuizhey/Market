using Market.Domain.Enums;

namespace Market.Application.Features.Products.Queries.GetLatestByType;

public class GetLatestProductsByTypeDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string CoverImagePath { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public ProductType ProductType { get; set; }
} 