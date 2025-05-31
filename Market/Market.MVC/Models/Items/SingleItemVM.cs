namespace Market.MVC.Models.Items;

public sealed class SingleItemVM
{
    public required string Title { get; set; }
    public required string Text { get; set; }
    public decimal Price { get; set; }
}