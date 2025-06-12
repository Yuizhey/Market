namespace Market.Application.Features.Products.Queries.GetAll;

public class GetAllProductsDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public decimal Price { get; set; }
    public string AuthorName { get; set; }
    public DateTime CreatedDate { get; set; }
    public string ProductType { get; set; }
} 