using MediatR;
using Mellon.Services.Application.Members;
using Mellon.Services.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mellon.Services.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public class MembersController : Controller
    {
        private readonly IMediator mediator;
        public MembersController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        [HttpGet]
        [Route("me")]
        [ProducesResponseType(typeof(ElectraUser), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Me()
        {
            var command = new GetCurrentMemberCommand();
            ElectraUser result = await mediator.Send(command);
            return Ok(result);
        }

    }
}
