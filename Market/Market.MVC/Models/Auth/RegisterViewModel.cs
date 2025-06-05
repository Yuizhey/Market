namespace Market.MVC.Models.Auth;

public sealed record RegisterViewModel
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string ConfirmPassword { get; init; }
    public required string UserName { get; init; }
}