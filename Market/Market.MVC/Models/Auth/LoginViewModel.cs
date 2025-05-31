namespace Market.MVC.Models.Auth;

public sealed class LoginViewModel
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}