using Market.Application.Interfaces.Repositories;
using Market.Domain.Entities;
using MediatR;

namespace Market.Application.Features.Products.Commands;

public class AddNewProductCommandHandler : IRequestHandler<AddNewProductCommand>
{
    private readonly IProductRepository _productRepository;
    
    public AddNewProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public async Task Handle(AddNewProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Title = request.Title,
            Text = request.Text,
            Price = request.Price,
            Id = Guid.NewGuid(),
        };
        await _productRepository.AddProductAsync(product);
    }
}