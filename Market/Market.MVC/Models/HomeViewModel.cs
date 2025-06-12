using Market.Application.Features.Products.Queries.GetLatestByType;

namespace Market.MVC.Models;

public class HomeViewModel
{
    public IEnumerable<GetLatestProductsByTypeDto> LatestUIKits { get; set; }
    public IEnumerable<GetLatestProductsByTypeDto> LatestWordPress { get; set; }
    public IEnumerable<GetLatestProductsByTypeDto> LatestHTML { get; set; }
} 