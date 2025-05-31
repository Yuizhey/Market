namespace Market.Application.Features.Products.Queries.GetByPageNumber;

public class GetByPageNumberDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Text { get; set; }
    public decimal Price { get; set; }
}