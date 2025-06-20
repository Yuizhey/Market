using System.ComponentModel.DataAnnotations;

namespace Market.MVC.Models.Auth;

public sealed class AuthorRegisterViewModel
{
    [Required(ErrorMessage = "Store name is required")]
    [StringLength(50, ErrorMessage = "Store name must be less than 50 characters")]
    public required string AuthorUserName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
    public required string Password { get; set; }
}