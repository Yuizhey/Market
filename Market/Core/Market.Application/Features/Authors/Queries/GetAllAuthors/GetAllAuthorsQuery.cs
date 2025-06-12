using MediatR;

namespace Market.Application.Features.Authors.Queries.GetAllAuthors;

public class GetAllAuthorsQuery : IRequest<IEnumerable<GetAllAuthorsResponse>>
{
} 