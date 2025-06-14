using Market.Application.Interfaces.Repositories;
using Market.Application.Interfaces.Services;
using Market.Domain.Entities;
using Market.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Market.Application.Features.Carts.Commands.CheckoutCart;

public class CheckoutCartCommandHandler : IRequestHandler<CheckoutCartCommand, Unit>
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

    public async Task<Unit> Handle(CheckoutCartCommand request, CancellationToken cancellationToken)
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
            
            var purchase = new Domain.Entities.Purchase
            {
                Id = Guid.NewGuid(),
                ProductId = item.ProductId,
                Price = item.Product.Price,
                BuyerId = buyerId,
                SellerId = item.Product.AuthorUserId,
                PurchaseDate = DateTime.UtcNow
            };
            await _purchaseRepository.AddPurchaseAsync(purchase);
        }

        await _cartRepository.DeleteCartAsync(cart.Id);

        // Отправляем чек на email с информацией о платеже
        var maskedCardNumber = $"**** **** **** {request.CardNumber.Substring(request.CardNumber.Length - 4)}";
        var emailBody = $@"
            <h2>Чек о покупке</h2>
            <p>Уважаемый(ая) {request.FirstName} {request.LastName},</p>
            <p>Спасибо за покупку!</p>
            <p>Детали платежа:</p>
            <ul>
                <li>Номер карты: {maskedCardNumber}</li>
                <li>Держатель карты: {request.CardHolderName}</li>
                <li>Срок действия: {request.CardExpiryMonth}/{request.CardExpiryYear}</li>
            </ul>
            <p>Список купленных товаров:</p>
            <ul>
                {string.Join("", cart.Items.Select(item => $"<li>{item.Product.Title} - ${item.Product.Price}</li>"))}
            </ul>
            <p>Общая сумма: ${cart.Items.Sum(item => item.Product.Price)}</p>";

        await _emailService.SendEmailAsync(request.Email, "Чек о покупке", emailBody);
        
        return Unit.Value;
    }
} 