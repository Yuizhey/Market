namespace Market.MVC.Views.Cart;

public class CartCheckOutVM
{
    public required string CartId { get; set; }
    public required string Email { get; set; }
    
    // Информация о покупателе
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PhoneNumber { get; set; }
    
    // Информация о платеже
    public required string CardHolderName { get; set; }
    public required string CardNumber { get; set; }
    public required string CardExpiryMonth { get; set; }
    public required string CardExpiryYear { get; set; }
    public required string CardCVC { get; set; }
    
    // Маскированный номер карты для чека
    public string MaskedCardNumber => $"**** **** **** {CardNumber.Substring(CardNumber.Length - 4)}";
}