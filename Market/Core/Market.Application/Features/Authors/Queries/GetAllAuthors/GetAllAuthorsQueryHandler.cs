using Market.Application.Interfaces.Repositories;
using MediatR;

namespace Market.Application.Features.Authors.Queries.GetAllAuthors;

public class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, IEnumerable<GetAllAuthorsResponse>>
{
    private readonly IAuthorUserDescriptionRepository _authorUserDescriptionRepository;

    public GetAllAuthorsQueryHandler(IAuthorUserDescriptionRepository authorUserDescriptionRepository)
    {
        _authorUserDescriptionRepository = authorUserDescriptionRepository;
    }

    public async Task<IEnumerable<GetAllAuthorsResponse>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
    {
        var authors = await _authorUserDescriptionRepository.GetAllAsync();
        
        return authors.Select(a => new GetAllAuthorsResponse
        {
            Id = a.Id,
            IdentityUserId = a.IdentityUserId,
            Email = a.Email,
            FirstName = a.FirstName,
            LastName = a.LastName,
            Gender = a.Gender,
            Phone = a.Phone,
        });
    }
} 