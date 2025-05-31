using Market.Application.Features.Carts.Queries.GetCartByUserId;

namespace Market.MVC.Models.Cart;

public class CartVM
{
    public Guid CartId { get; set; }
    public List<CartItemVM> Items { get; set; } = new();
}