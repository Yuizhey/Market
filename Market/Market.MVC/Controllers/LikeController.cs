using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Market.Application.Features.Likes.Commands;

namespace Market.MVC.Controllers
{
    [Authorize]
    public class LikeController : Controller
    {
        private readonly IMediator _mediator;

        public LikeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Like/Toggle/{productId}")]
        public async Task<IActionResult> Toggle(Guid productId)
        {
            await _mediator.Send(new ToggleLikeCommand(productId));
            return Ok();
        }
    }
}