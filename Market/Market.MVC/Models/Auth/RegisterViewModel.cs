using System.ComponentModel.DataAnnotations;

namespace Market.MVC.Models.Auth;

public sealed record RegisterViewModel
{
    [Required(ErrorMessage = "Email is required")] 
    [EmailAddress(ErrorMessage = "Invalid email address")] 
    public required string Email { get; init; }

    [Required(ErrorMessage = "Password is required")] 
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")] 
    public required string Password { get; init; }

    [Required(ErrorMessage = "Confirm password is required")] 
    [Compare("Password", ErrorMessage = "Passwords do not match")] 
    public required string ConfirmPassword { get; init; }

    [Required(ErrorMessage = "User name is required")] 
    [StringLength(50, ErrorMessage = "User name must be less than 50 characters")] 
    public required string UserName { get; init; }
}