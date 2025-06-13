using MediatR;

namespace Market.Application.Features.Users.Queries.GetAdmins;

public record GetAdminsQuery : IRequest<IEnumerable<AdminDto>>; 