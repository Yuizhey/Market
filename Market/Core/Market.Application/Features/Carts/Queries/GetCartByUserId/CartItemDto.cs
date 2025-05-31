namespace Market.Application.Features.Carts.Queries.GetCartByUserId;

public class CartItemDto
{
    public Guid ProductId { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
}