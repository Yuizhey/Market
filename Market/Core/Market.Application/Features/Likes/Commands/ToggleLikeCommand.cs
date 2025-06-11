using MediatR;

namespace Market.Application.Features.Likes.Commands;

public record ToggleLikeCommand(Guid ProductId) : IRequest;