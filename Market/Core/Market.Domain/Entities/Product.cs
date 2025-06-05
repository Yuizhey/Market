using Market.Domain.Common;

namespace Market.Domain.Entities;

public sealed class Product : BaseAuditableEntity
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Text { get; set; }
    public decimal Price { get; set; }
    public Guid AuthorUserId { get; set; } 
    public AuthorUserDescription? Author { get; set; }
    public string? CoverImagePath { get; set; }
}