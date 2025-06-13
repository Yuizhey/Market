using MediatR;

namespace Market.Application.Features.Users.Commands.DeleteAdmin;

public record DeleteAdminCommand(Guid Id) : IRequest; 