using Market.Domain.Common;
using Market.Domain.Enums;

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
    public ProductType ProductType { get; set; }
    public List<string> AdditionalFilePaths { get; set; } = new List<string>();

    public required string Subtitle { get; set; }
    public required string ShortDescription { get; set; }
}