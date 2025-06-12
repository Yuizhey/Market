using System.Security.Claims;
using Market.Application.Interfaces.Repositories;
using Market.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Market.Application.Features.Profile.Commands.AddUserDescription;

public class AddUserDescriptionCommandHandler : IRequestHandler<AddUserDescriptionCommand>
{
    private readonly IGenericRepository<UserDescription> _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserDescriptionRepository _UserDescriptionRepository;

    public AddUserDescriptionCommandHandler(
        IGenericRepository<UserDescription> repository,
        IHttpContextAccessor httpContextAccessor,
        IUserDescriptionRepository UserDescriptionRepository)
    {
        _repository = repository;
        _httpContextAccessor = httpContextAccessor;
        _UserDescriptionRepository = UserDescriptionRepository;
    }

    public async Task Handle(AddUserDescriptionCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var entity = new UserDescription
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Gender = request.Gender,
            Phone = request.Phone,
            IdentityUserId = Guid.Parse(userId),
        };
        await _UserDescriptionRepository.UpdateAsync(entity);
    }
}
