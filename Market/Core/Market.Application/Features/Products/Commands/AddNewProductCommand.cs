using MediatR;
using Microsoft.AspNetCore.Http;

namespace Market.Application.Features.Products.Commands;

public class AddNewProductCommand : IRequest
{
    public required string Title { get; set; }
    public required string Text { get; set; }
    public decimal Price { get; set; }
    public IFormFile? MainImage { get; set; }
}