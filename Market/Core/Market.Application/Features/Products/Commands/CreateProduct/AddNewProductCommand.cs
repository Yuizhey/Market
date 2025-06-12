using Market.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Market.Application.Features.Products.Commands;

public class AddNewProductCommand : IRequest
{
    public required string Title { get; set; }
    public required string Text { get; set; }
    public decimal Price { get; set; }
    public Guid AuthorUserId { get; set; } 
    public IFormFile? CoverImage { get; set; }
    public IFormFile[]? AdditionalFiles { get; set; }
    public required string Subtitle { get; set; }
    public required string ShortDescription { get; set; }
    public required ProductType ProductType { get; set; }
}