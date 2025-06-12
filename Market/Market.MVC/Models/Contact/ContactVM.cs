using Market.Domain.Enums;

namespace Market.MVC.Models.Contact;

public class ContactVM
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Message { get; set; }
    public ContactType Type { get; set; }
}