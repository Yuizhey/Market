using System.ComponentModel.DataAnnotations;
using Market.Domain.Enums;

namespace Market.MVC.Models.Profile;

public class UserDescriptionVM
{
    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, ErrorMessage = "First name must be less than 50 characters")]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, ErrorMessage = "Last name must be less than 50 characters")]
    public required string LastName { get; set; }

    public Gender Gender { get; set; }

    [Required(ErrorMessage = "Phone is required")]
    [Phone(ErrorMessage = "Invalid phone number")]
    public required string Phone { get; set; }
}