using Market.Application.Interfaces.Repositories;
using Market.Application.Interfaces.Services;
using Market.Domain.Entities;
using Market.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Market.Application.Features.Carts.Commands.CheckoutCart;

public class CheckoutCartCommandHandler : IRequestHandler<CheckoutCartCommand>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductSaleStatisticsRepository _productSaleStatisticsRepository;
    private readonly IEmailService _emailService;
    private readonly IPurchaseRepository _purchaseRepository;
    private readonly IUserDescriptionRepository _userDescriptionRepository;
    private readonly IAuthorUserDescriptionRepository _authorUserDescriptionRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CheckoutCartCommandHandler(
        ICartRepository cartRepository,
        IProductSaleStatisticsRepository productSaleStatisticsRepository,
        IEmailService emailService,
        IPurchaseRepository purchaseRepository,
        IUserDescriptionRepository userDescriptionRepository,
        IAuthorUserDescriptionRepository authorUserDescriptionRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _cartRepository = cartRepository;
        _productSaleStatisticsRepository = productSaleStatisticsRepository;
        _emailService = emailService;
        _purchaseRepository = purchaseRepository;
        _userDescriptionRepository = userDescriptionRepository;
        _authorUserDescriptionRepository = authorUserDescriptionRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task Handle(CheckoutCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByIdAsync(request.CartId);
        if (cart == null)
            throw new InvalidOperationException("Cart not found");

        if (!cart.Items.Any())
            throw new InvalidOperationException("Cart is empty");

        var identityUserId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);

        Guid buyerId;
        if (userRole == UserRoles.CLientUser.ToString())
        {
            var userDescriptionId = await _userDescriptionRepository.GetBusinessIdByIdentityUserIdAsync(Guid.Parse(identityUserId!));
            if (userDescriptionId == null)
                throw new InvalidOperationException("User description not found");
            buyerId = userDescriptionId;
        }
        else if (userRole == UserRoles.AuthorUser.ToString())
        {
            var authorUserDescriptionId = await _authorUserDescriptionRepository.GetBusinessIdByIdentityUserIdAsync(Guid.Parse(identityUserId!));
            if (authorUserDescriptionId == null)
                throw new InvalidOperationException("Author user description not found");
            buyerId = authorUserDescriptionId;
        }
        else
        {
            throw new InvalidOperationException("Invalid user role");
        }

        foreach (var item in cart.Items)
        {
            await _productSaleStatisticsRepository.UpdateProductStatisticsAsync(item.ProductId, item.Product.Price);
            
            // Проверяем, не пытается ли автор купить свой собственный товар
            if (item.Product.AuthorUserId == buyerId)
                throw new InvalidOperationException("You cannot buy your own product");

            var purchase = new Domain.Entities.Purchase
            {
                Id = Guid.NewGuid(),
                ProductId = item.ProductId,
                Price = item.Product.Price,
                BuyerId = buyerId,
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