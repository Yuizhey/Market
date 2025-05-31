using Market.Domain.Common;

namespace Market.Domain.Entities;

public sealed class Cart : BaseAuditableEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    public List<CartItem> Items { get; set; } = new();
}