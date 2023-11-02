using MediatR;
using Mellon.Common.Services;
using Mellon.Services.Application.Contact;
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
    public class ContactsController : Controller
    {
        private readonly IMediator mediator;
        public ContactsController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [Route("list")]
        [ProducesResponseType(typeof(PaginatedListResult<ContactResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> List([FromQuery] string term, [FromQuery] QueryPaging paging, [FromQuery] QueryOrder order)
        {
            var command = new GetContactsCommand(
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
        [ProducesResponseType(typeof(ContactResource), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMember(int id)
        {
            var command = new GetContactCommand(id);
            ContactResource result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [Route("")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(ContactResource), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ContactResource), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateContact([FromBody] ContactResourceData request)
        {
            var command = new ContactCreateCommand(request);
            ContactResource result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        [Route("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(ContactResource), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateContact(int id, [FromBody] ContactResource request)
        {
            var command = new ContactUpdateCommand(request);
            ContactResource result = await mediator.Send(command);
            return Ok(result);
        }
    }
}
