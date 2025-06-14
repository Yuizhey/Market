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
using System.Security.Claims;

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

    [HttpPost]
    public async Task<IActionResult> Checkout(CartCheckOutVM model)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", await GetCartViewModel());
        }

        try
        {
            var command = new CheckoutCartCommand
            {
                CartId = Guid.Parse(model.CartId),
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                CardHolderName = model.CardHolderName,
                CardNumber = model.CardNumber,
                CardExpiryMonth = model.CardExpiryMonth,
                CardExpiryYear = model.CardExpiryYear,
                CardCVC = model.CardCVC
            };

            await _mediator.Send(command);
            return RedirectToAction("MyDownloads", "Profile");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View("Index", await GetCartViewModel());
        }
    }

    [HttpPost("/Cart/RemoveItem")]
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

    private async Task<CartVM> GetCartViewModel()
    {
        var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(identityUserId))
        {
            return new CartVM { Items = new List<CartItemVM>() };
        }

        var cart = await _mediator.Send(new GetUserCartQuery());
        if (cart == null)
        {
            return new CartVM { Items = new List<CartItemVM>() };
        }

        return new CartVM
        {
            CartId = cart.CartId,
            Items = cart.Items.Select(item => new CartItemVM
            {
                ProductId = item.ProductId,
                Title = item.Title,
                Price = item.Price
            }).ToList()
        };
    }
}