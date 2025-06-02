using Market.Application.Features.Products.Queries.GetByPageNumber;

namespace Market.MVC.Models.Items;

public sealed class ItemsVM
{
    public IEnumerable<GetByPageNumberDto> Products { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
}
