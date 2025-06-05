using System.Security.Claims;
using Market.Application.Interfaces.Repositories;
using Market.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Market.Application.Features.Products.Queries.GetByUserId;

public class GetByUserIdQueryHandler : IRequestHandler<GetByUserIdQuery, IEnumerable<GetByUserIdDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuthorUserDescriptionRepository _authorUserDescriptionRepository;

    public GetByUserIdQueryHandler(IProductRepository productRepository, IHttpContextAccessor httpContextAccessor, IAuthorUserDescriptionRepository authorUserDescriptionRepository)
    {
        _productRepository = productRepository;
        _httpContextAccessor = httpContextAccessor;
        _authorUserDescriptionRepository = authorUserDescriptionRepository;
    }
    
    public async Task<IEnumerable<GetByUserIdDto>> Handle(GetByUserIdQuery request, CancellationToken cancellationToken)
    {
        var identityUserId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var authorUserDescriptionId = await _authorUserDescriptionRepository.GetBusinessIdByIdentityUserIdAsync(Guid.Parse(identityUserId!));
            if (authorUserDescriptionId == null)
                throw new InvalidOperationException("Author user description not found");
        var entities = await _productRepository.GetProductsByUserId(authorUserDescriptionId);
        var products = entities.Select(p => new GetByUserIdDto
        {
            Id = p.Id,
            Title = p.Title,
            Price = p.Price,
        });
        return products;
    }
}