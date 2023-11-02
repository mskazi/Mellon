using MediatR;
using Mellon.Common.Services;
using Mellon.Services.Application.Approvals;
using Mellon.Services.Application.Services;
using Mellon.Services.Application.Vouchers.Commands;
using Mellon.Services.Common.resources;
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
    public class VouchersController : Controller
    {
        private readonly IMediator mediator;
        public VouchersController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        [HttpGet]
        [Route("service/vouchers")]
        [ProducesResponseType(typeof(ElectraUser), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ServiceVouchers([FromQuery] string term, [FromQuery] QueryPaging paging, [FromQuery] QueryOrder order)
        {
            var command = new GetVoucherServiceCommand(
                term,
                new ListPaging(paging),
                new ListOrder(order)
                );
            var result = await mediator.Send(command);
            return Ok(result);
        }


        // GET: api/<ApprovalsController>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ApprovalOrderResource), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var command = new GetVoucherDetailsCommand(id);
            VoucherDetails result = await mediator.Send(command);
            return Ok(result);
        }
    }
}
