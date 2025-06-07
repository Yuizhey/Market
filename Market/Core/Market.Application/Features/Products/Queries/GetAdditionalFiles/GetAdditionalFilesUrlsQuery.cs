using MediatR;

namespace Market.Application.Features.Products.Queries.GetAdditionalFiles;

public class GetAdditionalFilesUrlsQuery : IRequest<List<string>>
{
    public Guid ProductId { get; set; }

    public GetAdditionalFilesUrlsQuery(Guid productId)
    {
        ProductId = productId;
    }
}