using MediatR;

namespace Market.Application.Features.Users.Commands.CreateAdmin;

public record CreateAdminCommand(string Email, string Password) : IRequest; 