namespace Market.MVC.Models.Cart;

public class CartItemVM
{
    public Guid ProductId { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
}