using Market.Domain.Enums;

namespace Market.MVC.Models.Items;

public class AddItemVM
{
    public required string Title { get; set; }
    public required string Text { get; set; }
    public decimal Price { get; set; }
    public IFormFile? CoverImage { get; set; }
    public IFormFile[]? AdditionalFiles { get; set; }
    public required string Subtitle { get; set; }
    public required string ShortDescription { get; set; }
    public required ProductType ProductType { get; set; }
}