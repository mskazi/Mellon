using MediatR;
using Mellon.Services.Application.Approvals;
using Mellon.Services.Infrastracture.ModelExtensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
    public class ApprovalsController : ControllerBase
    {
        private readonly IMediator mediator;

        public ApprovalsController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        // GET: api/<ApprovalsController>
        [HttpGet]
        [Route("{documentToken}")]
        [ProducesResponseType(typeof(ApprovalOrderResource), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(string documentToken)
        {
            var command = new GetApprovalCommand(documentToken);
            ApprovalOrderResource result = await mediator.Send(command);
            return Ok(result);
        }


        // POST: api/<ApprovalsController>
        [HttpPost]
        [Route("decision/{documentToken}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Decision(string documentToken, [FromBody] ApprovalDecisionModel approvalDecision)
        {
            var command = new ApprovalDecisionCommand(documentToken, approvalDecision.comment ?? "", approvalDecision.decision);
            bool result = await mediator.Send(command);
            return Ok(result);
        }
       
        public class ApprovalDecisionModel
        {
            public ApprovalStatusEnum decision { get; set; }
            public string? comment { get; set; }
        }
    }
}
