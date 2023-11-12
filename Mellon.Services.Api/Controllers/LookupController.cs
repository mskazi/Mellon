using MediatR;
using Mellon.Common.Services;
using Mellon.Services.Application.Lookup;
using Mellon.Services.Application.Lookup.Commands;
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
    public class LookupController : Controller
    {
        private readonly IMediator mediator;
        public LookupController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        [HttpGet]
        [Route("departments")]
        [ProducesResponseType(typeof(ListResult<DepartmentLookupResourse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDepartments()
        {
            var command = new GetDepartmentookupCommand();
            var result = await mediator.Send(command);
            return Ok(result);
        }


        [HttpGet]
        [Route("companies")]
        [ProducesResponseType(typeof(ListResult<CompanyLookupResourse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCompanies()
        {
            var command = new GeCompanyLookupCommand();
            var result = await mediator.Send(command);
            return Ok(result);
        }


        [HttpGet]
        [Route("countries")]
        [ProducesResponseType(typeof(ListResult<CountryLookupResourse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCountries()
        {
            var command = new GeCountryLookupCommand();
            var result = await mediator.Send(command);
            return Ok(result);
        }


        [HttpGet]
        [Route("types/office")]
        [ProducesResponseType(typeof(ListResult<TypeLookupResourse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOfficeType()
        {
            var command = new GetOfficeTypeLookupCommand();
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        [Route("conditions/office")]
        [ProducesResponseType(typeof(ListResult<ConditionLookupResourse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOfficeCondition()
        {
            var command = new GetOfficeConditionLookupCommand();
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        [Route("departments/office")]
        [ProducesResponseType(typeof(ListResult<DepartmentLookupResourse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOfficeDepartment()
        {
            var command = new GetOfficeDepartmentLookupCommand();
            var result = await mediator.Send(command);
            return Ok(result);
        }



        [HttpGet]
        [Route("deliveryTimes/office")]
        [ProducesResponseType(typeof(ListResult<ConditionLookupResourse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOfficeDeliveryTimes()
        {
            var command = new GetOfficeDeliveryTimeLookupCommand();
            var result = await mediator.Send(command);
            return Ok(result);
        }
    }
}
