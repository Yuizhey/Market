using Market.Application.Features.Products.Queries.GetByPageNumber;

namespace Market.MVC.Models.Items;

public class ItemsVM
{
    public IEnumerable<GetByPageNumberDto> Products { get; set; }
}