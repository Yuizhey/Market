using Market.Application.Features.Products.Queries.GetLatestByType;

namespace Market.MVC.Models;

public class HomeViewModel
{
    public IEnumerable<GetLatestProductsByTypeDto> latestWeb { get; set; }
    public IEnumerable<GetLatestProductsByTypeDto> latestFonts { get; set; }
    public IEnumerable<GetLatestProductsByTypeDto> latestGraphics { get; set; }
} 