using Market.Application.Features.Purchase.Queries.GetUserPurchases;

namespace Market.MVC.Models.Profile;

public class MyDownloadsVM
{
    public IEnumerable<PurchaseDto> Downloads { get; set; }
}