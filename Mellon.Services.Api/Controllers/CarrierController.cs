using MediatR;
using Mellon.Common.Services;
using Mellon.Services.Application.Carrier;
using Mellon.Services.Application.Carrier.Commands;
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
    public class CarrierController : Controller
    {
        private readonly IMediator mediator;
        public CarrierController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        [HttpGet]
        [Route("list")]
        [ProducesResponseType(typeof(ListResult<CarrierLookupResourse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCarriers([FromQuery] string postalcode)
        {
            var command = new GetCarriersLookupCommand(
               postalcode
                );
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        [Route("parameters")]
        [ProducesResponseType(typeof(CarrierLookupResourse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCarrierByPostalCode([FromQuery] string postalcode)
        {
            var command = new GetCarrierPostalCodeCommand(
               postalcode
                );
            var result = await mediator.Send(command);
            return Ok(result);
        }


    }
}
