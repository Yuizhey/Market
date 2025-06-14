using Market.Domain.Enums;
using MediatR;

namespace Market.Application.Features.Products.Queries.GetByPageNumber;

public class GetByPageNumberQuery : IRequest<GetByPageNumberREsult>
{
    public int Page { get; }
    public int PageSize { get; }
    public IEnumerable<ProductType>? ProductTypes { get; }
    public decimal? MinPrice { get; }
    public decimal? MaxPrice { get; }
    public string? SearchTerm { get; }

    public GetByPageNumberQuery(
        int page, 
        int pageSize, 
        IEnumerable<ProductType>? productTypes = null, 
        decimal? minPrice = null, 
        decimal? maxPrice = null,
        string? searchTerm = null)
    {
        Page = page;
        PageSize = pageSize;
        ProductTypes = productTypes;
        MinPrice = minPrice;
        MaxPrice = maxPrice;
        SearchTerm = searchTerm;
    }
}

