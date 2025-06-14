using MediatR;

namespace Market.Application.Features.Carts.Commands.CheckoutCart;

public class CheckoutCartCommand : IRequest<Unit>
{
    public Guid CartId { get; set; }
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string CardHolderName { get; set; } = null!;
    public string CardNumber { get; set; } = null!;
    public string CardExpiryMonth { get; set; } = null!;
    public string CardExpiryYear { get; set; } = null!;
    public string CardCVC { get; set; } = null!;
} 