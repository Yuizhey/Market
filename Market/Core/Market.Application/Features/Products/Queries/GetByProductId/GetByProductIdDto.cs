namespace Market.Application.Features.Products.Queries.GetByProductId;

public class GetByProductIdDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string? Subtitle { get; set; }
    public required string Text { get; set; }
    public string ShortDescription { get; set; }
    public decimal Price { get; set; }
    public string ImageURL { get; set; }
    public int LikesCount { get; set; }
    public bool IsLiked { get; set; }
}