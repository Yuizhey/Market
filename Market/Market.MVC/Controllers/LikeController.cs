using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Market.Application.Features.Likes.Commands;
using Microsoft.Extensions.Logging;

namespace Market.MVC.Controllers
{
    [Authorize]
    public class LikeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<LikeController> _logger;

        public LikeController(IMediator mediator, ILogger<LikeController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("Like/Toggle/{productId}")]
        public async Task<IActionResult> Toggle(Guid productId)
        {
            _logger.LogInformation("Переключение лайка для товара: {ProductId}", productId);
            await _mediator.Send(new ToggleLikeCommand(productId));
            _logger.LogInformation("Лайк успешно переключен для товара: {ProductId}", productId);
            return Ok();
        }
    }
}