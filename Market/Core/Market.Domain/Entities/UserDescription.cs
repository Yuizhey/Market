using System.Net.NetworkInformation;
using Market.Domain.Enums;

namespace Market.Domain.Entities;

public class UserDescription
{
    public Guid Id { get; set; }
    public Guid IdentityUserId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public Gender Gender { get; set; }
    public required string Phone { get; set; }
}