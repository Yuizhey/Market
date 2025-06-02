using Market.Domain.Enums;

namespace Market.MVC.Models.Profile;

public class UserDescriptionVM
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public Gender Gender { get; set; }
    public required string Phone { get; set; }
}