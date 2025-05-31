using System.Security.Claims;
using Market.Application.Features.Carts.Queries.GetCartByUserId;
using Market.MVC.Models.Cart;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Market.MVC.Controllers;

public class CartController : Controller
{
    private readonly IMediator _mediator;
    public CartController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task<IActionResult> Index()
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var dto = await _mediator.Send(new GetUserCartQuery(userId));

        var viewModel = new CartVM
        {
            CartId = dto.CartId,
            Items = dto.Items.Select(i => new CartItemVM
            {
                ProductId = i.ProductId,
                Title = i.Title,
                Price = i.Price
            }).ToList()
        };

        return View(viewModel);
    }

}