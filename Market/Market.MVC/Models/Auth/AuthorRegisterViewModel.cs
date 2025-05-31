namespace Market.MVC.Models.Auth;

public sealed class AuthorRegisterViewModel
{
    public required string AuthorUserName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}