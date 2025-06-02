using Market.Domain.Enums;

namespace Market.MVC.Models.Profile;

public class AuthorUserDescriptionVM
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Country { get; set; }
    public Gender Gender { get; set; }
    public required string Phone { get; set; }
    public required string HomeAddress { get; set; }
    public required string Address { get; set; }
}