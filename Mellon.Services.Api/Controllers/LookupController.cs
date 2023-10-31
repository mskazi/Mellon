using MediatR;
using Mellon.Common.Services;
using Mellon.Services.Application.Lookup;
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


        // GET: api/<ApprovalsController>
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
    }
}
