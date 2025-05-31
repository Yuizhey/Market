using Market.Domain.Common;

namespace Market.Domain.Entities;

public sealed class CartItem : BaseAuditableEntity
{
    public Guid Id { get; set; }
    
    public Guid CartId { get; set; }
    public Cart Cart { get; set; }

    public Guid ProductId { get; set; }
    public Product Product { get; set; }
}