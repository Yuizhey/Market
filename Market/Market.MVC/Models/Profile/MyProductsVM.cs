using Market.Application.Features.Auth.Queries.Login;
using GetByUserIdDto = Market.Application.Features.Products.Queries.GetByUserId.GetByUserIdDto;

namespace Market.MVC.Models.Profile;

public class MyProductsVM
{
    public IEnumerable<GetByUserIdDto> MyProducts { get; set; }
}