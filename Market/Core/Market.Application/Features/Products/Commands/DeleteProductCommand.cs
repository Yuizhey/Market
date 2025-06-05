using MediatR;

namespace Market.Application.Features.Products.Commands;

public record DeleteProductCommand(Guid ProductId) : IRequest; 