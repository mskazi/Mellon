using MediatR;
using Mellon.Common.Services;
using Mellon.Services.Application;
using Mellon.Services.Application.Member;
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


        [HttpGet]
        [Route("list")]
        [ProducesResponseType(typeof(ElectraUser), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> List([FromQuery] string term, [FromQuery] QueryPaging paging, [FromQuery] QueryOrder order)
        {
            var command = new GetMembersServiceCommand(
                term,
                new ListPaging(paging),
                new ListOrder(order)
                );
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(MemberResource), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMember(int id)
        {
            var command = new GetMemberCommand(id);
            MemberResource result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [Route("")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(MemberResource), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(MemberResource), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateMember([FromBody] MemberResourceData request)
        {
            var command = new MemberCreateCommand(request.MemberName, request.Department, request.Company, request.SysCountry, request.IsActive);
            MemberResource result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        [Route("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(MemberResource), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateMember(int id, [FromBody] MemberResource request)
        {
            var command = new MemberUpdateCommand(request.Id, request.MemberName, request.Department, request.Company, request.SysCountry, request.IsActive);
            MemberResource result = await mediator.Send(command);
            return Ok(result);
        }
    }
}
