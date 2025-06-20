using System.ComponentModel.DataAnnotations;
using Market.Domain.Enums;

namespace Market.MVC.Models.Profile;

public class AuthorUserDescriptionVM
{
    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, ErrorMessage = "First name must be less than 50 characters")]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, ErrorMessage = "Last name must be less than 50 characters")]
    public required string LastName { get; set; }

    [Required(ErrorMessage = "Country is required")]
    public required string Country { get; set; }

    public Gender Gender { get; set; }

    [Required(ErrorMessage = "Phone is required")]
    [Phone(ErrorMessage = "Invalid phone number")]
    public required string Phone { get; set; }

    [Required(ErrorMessage = "Home address is required")]
    public required string HomeAddress { get; set; }

    [Required(ErrorMessage = "Office address is required")]
    public required string Address { get; set; }
}