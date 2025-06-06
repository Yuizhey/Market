using Market.Domain.Common;

namespace Market.Domain.Entities;

public sealed class Purchase : BaseAuditableEntity
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }
    public Product Product { get; set; }

    public Guid BuyerUserId { get; set; }
    public UserDescription Buyer { get; set; }

    public Guid SellerUserId { get; set; }
    public AuthorUserDescription Seller { get; set; }

    public decimal Price { get; set; }  
    public DateTime PurchaseDate { get; set; }

    public int Quantity { get; set; } = 1;
}