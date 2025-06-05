using System.Security.Claims;
using Market.Application.Features.Carts.Commands.AddItemToCart;
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
        var dto = await _mediator.Send(new GetUserCartQuery());

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

    [HttpPost("Cart/AddItemToCart/{productId}")]
    public async Task<IActionResult> AddItemToCart([FromRoute]Guid productId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var command = new AddItemToCartCommand(userId, productId);
        await _mediator.Send(command);

        return Ok();
    }
}