using MediatR;
using Mellon.Common.Services;
using Mellon.Services.Application.Vouchers;
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
        [Route("service")]
        [ProducesResponseType(typeof(PaginatedListResult<VoucherServiceItem>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> VouchersService([FromQuery] string term, [FromQuery] QueryPaging paging, [FromQuery] QueryOrder order)
        {
            var command = new GetVoucherServiceCommand(
                term,
                new ListPaging(paging),
                new ListOrder(order)
                );
            var result = await mediator.Send(command);
            return Ok(result);
        }


        [HttpGet]
        [Route("office")]
        [ProducesResponseType(typeof(PaginatedListResult<VoucherOfficeItem>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> VouchersOffice([FromQuery] string term, [FromQuery] QueryPaging paging, [FromQuery] QueryOrder order)
        {
            var command = new GetVoucherOfficeCommand(
                term,
                new ListPaging(paging),
                new ListOrder(order)
                );
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        [Route("warehouse")]
        [ProducesResponseType(typeof(PaginatedListResult<VoucherWarehouseItem>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> VouchersWarehouse([FromQuery] string term, [FromQuery] QueryPaging paging, [FromQuery] QueryOrder order)
        {
            var command = new GetVoucherWarehouseCommand(
                term,
                new ListPaging(paging),
                new ListOrder(order)
                );
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        [Route("search")]
        [ProducesResponseType(typeof(PaginatedListResult<VoucherSearchItem>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> VouchersSearch([FromQuery] string term, [FromQuery] QueryPaging paging, [FromQuery] QueryOrder order)
        {
            var command = new GetVoucherSearchCommand(
                term,
                new ListPaging(paging),
                new ListOrder(order)
                );
            var result = await mediator.Send(command);
            return Ok(result);
        }


        // GET: api/<ApprovalsController>
        [HttpGet]
        [Route("summary")]
        [ProducesResponseType(typeof(VoucherSummary), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSummary()
        {
            var command = new GetSummaryCommand();
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        [Route("track/{id}")]
        [ProducesResponseType(typeof(VoucherSummary), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTrack(int id)
        {
            var command = new GetVoucherTrackCommand(id);
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        [Route("details/{id}")]
        [ProducesResponseType(typeof(VoucherDetails), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetails(int id)
        {
            var command = new GetVoucherDetailsCommand(id);
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [Route("print/{id}")]
        [ProducesResponseType(typeof(VoucherDetails), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Print(int id)
        {
            var command = new GetVoucherPrintCommand(id);
            var result = await mediator.Send(command);
            return File(result, "application/pdf");
        }

        [HttpPost]
        [Route("cancel/{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Cancel(int id)
        {
            var command = new GetVoucherCancelCommand(id);
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [Route("contact/new/{id}")]
        [ProducesResponseType(typeof(Boolean), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateVoucherFromContact([FromRoute] int id, [FromQuery] VoucherCreateRoleType roleType, [FromBody] CreateVoucherRequestData data)
        {
            var command = new CreateVoucherFromContactCommand(id, roleType, data);
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [Route("service/scan")]
        [ProducesResponseType(typeof(Boolean), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateVoucherServiceScan([FromBody] CreateVoucherServiceScanData data)
        {
            var command = new CreateVoucherFromServiceScanCommand(data);
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [Route("service/order")]
        [ProducesResponseType(typeof(Boolean), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PrepareVoucherServiceOrder([FromQuery] string order)
        {
            var command = new GetVoucherServiceOrderInfoCommand(order);
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [Route("service/order/create")]
        [ProducesResponseType(typeof(Boolean), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateVoucherServiceOrder([FromQuery] string order)
        {
            var command = new CreateVoucherServiceOrderCommand(order);
            var result = await mediator.Send(command);
            return Ok(result);
        }


        [HttpGet]
        [Route("office/vouchers")]
        [ProducesResponseType(typeof(Boolean), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVouchersOfficeByCompany([FromQuery] string company)
        {
            var command = new GetVouchersOfficeByCompany(company);
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        [Route("office/vouchers/print")]
        [ProducesResponseType(typeof(Boolean), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PrintVouchersOffice([FromQuery] IEnumerable<string> vouchers)
        {
            var command = new VouchersOfficePrint(vouchers);
            var result = await mediator.Send(command);
            return File(result, "application/pdf");
        }
    }
}
