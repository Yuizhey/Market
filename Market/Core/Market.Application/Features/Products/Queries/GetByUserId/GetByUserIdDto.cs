namespace Market.Application.Features.Products.Queries.GetByUserId;

public class GetByUserIdDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public decimal Price { get; set; }
}