using Market.Application.Features.Carts.Commands.AddItemToCart;
using Market.Application.Features.Carts.Commands.CheckoutCart;
using Market.Application.Features.Carts.Queries.GetCartByUserId;
using Market.Application.Features.Carts.Commands.RemoveItem;
using Market.MVC.Models.Cart;
using Market.MVC.Views.Cart;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Market.MVC.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<CartController> _logger;

    public CartController(IMediator mediator, ILogger<CartController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    
    public async Task<IActionResult> Index()
    {
        _logger.LogInformation("Просмотр корзины пользователя");
        
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
        _logger.LogInformation("Добавление товара в корзину: {ProductId}", productId);
        
        var command = new AddItemToCartCommand(productId);
        await _mediator.Send(command);
        
        _logger.LogInformation("Товар успешно добавлен в корзину: {ProductId}", productId);
        return Ok();
    }

    [HttpPost("Cart/Checkout")]
    public async Task<IActionResult> Checkout([FromForm]CartCheckOutVM model)
    {
        _logger.LogInformation("Оформление заказа. Email: {Email}", model.Email);
        
        await _mediator.Send(new CheckoutCartCommand(Guid.Parse(model.CartId), model.Email));
        
        _logger.LogInformation("Заказ успешно оформлен. Email: {Email}", model.Email);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> RemoveItem([FromBody] RemoveItemCommand command)
    {
        try
        {
            await _mediator.Send(command);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}