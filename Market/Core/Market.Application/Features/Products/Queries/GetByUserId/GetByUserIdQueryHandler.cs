using System.Security.Claims;
using Market.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Market.Application.Features.Products.Queries.GetByUserId;

public class GetByUserIdQueryHandler : IRequestHandler<GetByUserIdQuery, IEnumerable<GetByUserIdDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetByUserIdQueryHandler(IProductRepository productRepository, IHttpContextAccessor httpContextAccessor)
    {
        _productRepository = productRepository;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<IEnumerable<GetByUserIdDto>> Handle(GetByUserIdQuery request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var authorUserId))
        {
            throw new UnauthorizedAccessException("Пользователь не аутентифицирован.");
        }
        var entities = await _productRepository.GetProductsByUserId(authorUserId);
        var products = entities.Select(p => new GetByUserIdDto
        {
            Id = p.Id,
            Title = p.Title,
            Price = p.Price,
        });
        return products;
    }
}