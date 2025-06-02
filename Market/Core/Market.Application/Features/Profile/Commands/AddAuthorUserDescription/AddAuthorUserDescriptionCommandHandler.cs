using System.Security.Claims;
using Market.Application.Interfaces.Repositories;
using Market.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Market.Application.Features.Profile.Commands.AddAuthorUserDescription;

public class AddAuthorUserDescriptionCommandHandler : IRequestHandler<AddAuthorUserDescriptionCommand>
{
    private readonly IGenericRepository<AuthorUserDescription> _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AddAuthorUserDescriptionCommandHandler(
        IGenericRepository<AuthorUserDescription> repository,
        IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task Handle(AddAuthorUserDescriptionCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var entity = new AuthorUserDescription
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Gender = request.Gender,
            Phone = request.Phone,
            Country = request.Country,
            Address = request.Address,
            HomeAddress = request.HomeAddress,
            IdentityUserId = Guid.Parse(userId),
        };
        await _repository.AddAsync(entity);
    }
}
