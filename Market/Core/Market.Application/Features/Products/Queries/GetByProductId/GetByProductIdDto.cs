namespace Market.Application.Features.Products.Queries.GetByProductId;

public class GetByProductIdDto
{
    public required string Title { get; set; }
    public required string Text { get; set; }
    public decimal Price { get; set; }
    public string ImageURL { get; set; }
}