namespace Market.Application.Features.Carts.Queries.GetCartByUserId;

public class GetUserCartDto
{
    public Guid CartId { get; set; }
    public List<CartItemDto> Items { get; set; } = new();
}