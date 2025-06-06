using Market.Application.Interfaces.Repositories;
using Market.Application.Interfaces.Services;
using Market.Domain.Entities;
using MediatR;

namespace Market.Application.Features.Carts.Commands.CheckoutCart;

public class CheckoutCartCommandHandler : IRequestHandler<CheckoutCartCommand>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductSaleStatisticsRepository _productSaleStatisticsRepository;
    private readonly IEmailService _emailService;
    private readonly IPurchaseRepository _purchaseRepository;

    public CheckoutCartCommandHandler(
        ICartRepository cartRepository,
        IProductSaleStatisticsRepository productSaleStatisticsRepository,
        IEmailService emailService,
        IPurchaseRepository purchaseRepository)
    {
        _cartRepository = cartRepository;
        _productSaleStatisticsRepository = productSaleStatisticsRepository;
        _emailService = emailService;
        _purchaseRepository = purchaseRepository;
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
            var purchase = new Purchase
            {
                Id = Guid.NewGuid(),
                ProductId = item.ProductId,
                Price = item.Product.Price,
                BuyerId = cart.UserId,
                SellerId = item.Product.AuthorUserId,
                PurchaseDate = DateTime.UtcNow,
            };
            await _purchaseRepository.AddPurchaseAsync(purchase);
        }

        await _cartRepository.DeleteCartAsync(cart.Id);
        
        var message = $"Спасибо за покупку!\n\nВаши товары:\n";
        foreach (var item in cart.Items)
        {
            message += $"- {item.Product.Title}: {item.Product.Price} ₽\n";
        }
        message += $"\nОбщая сумма: {cart.Items.Sum(i => i.Product.Price)} ₽";

        await _emailService.SendEmailAsync(request.Email, "Спасибо за покупку!", message);
    }
} 