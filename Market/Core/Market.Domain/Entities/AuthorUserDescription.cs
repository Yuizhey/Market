using Market.Domain.Common;
using Market.Domain.Enums;

namespace Market.Domain.Entities;

public class AuthorUserDescription : BaseAuditableEntity
{
    public Guid Id { get; set; }
    public Guid IdentityUserId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Country { get; set; }
    public Gender Gender { get; set; }
    public required string Phone { get; set; }
    public required string HomeAddress { get; set; }
    public required string Address { get; set; }
}