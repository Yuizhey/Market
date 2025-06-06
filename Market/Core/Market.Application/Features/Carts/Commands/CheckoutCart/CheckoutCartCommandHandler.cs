using Market.Application.Interfaces.Repositories;
using Market.Application.Interfaces.Services;
using MediatR;

namespace Market.Application.Features.Carts.Commands.CheckoutCart;

public class CheckoutCartCommandHandler : IRequestHandler<CheckoutCartCommand>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductSaleStatisticsRepository _productSaleStatisticsRepository;
    private readonly IEmailService _emailService;

    public CheckoutCartCommandHandler(
        ICartRepository cartRepository,
        IProductSaleStatisticsRepository productSaleStatisticsRepository,
        IEmailService emailService)
    {
        _cartRepository = cartRepository;
        _productSaleStatisticsRepository = productSaleStatisticsRepository;
        _emailService = emailService;
    }

    public async Task Handle(CheckoutCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByIdAsync(request.CartId);
        if (cart == null)
            throw new InvalidOperationException("Cart not found");

        if (!cart.Items.Any())
            throw new InvalidOperationException("Cart is empty");

        foreach (var item in cart.Items)
        {
            await _productSaleStatisticsRepository.UpdateProductStatisticsAsync(item.ProductId, item.Product.Price);
        }

        var message = $"Спасибо за покупку!\n\nВаши товары:\n";
        foreach (var item in cart.Items)
        {
            message += $"- {item.Product.Title}: {item.Product.Price} ₽\n";
        }
        message += $"\nОбщая сумма: {cart.Items.Sum(i => i.Product.Price)} ₽";

        await _emailService.SendEmailAsync(request.Email, "Спасибо за покупку!", message);

        await _cartRepository.DeleteCartAsync(cart.Id);
    }
} 