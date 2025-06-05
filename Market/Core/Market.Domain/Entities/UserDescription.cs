using System.Net.NetworkInformation;
using Market.Domain.Common;
using Market.Domain.Enums;

namespace Market.Domain.Entities;

public class UserDescription : BaseAuditableEntity
{
    public Guid Id { get; set; }
    public Guid IdentityUserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Gender? Gender { get; set; }
    public string? Phone { get; set; }
}