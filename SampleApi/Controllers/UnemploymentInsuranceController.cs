using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sample.Api.Configuration;
using Sample.Application.UnemploymentInsurances.Dto;
using Sample.Application.UnemploymentInsurances.Gets;
using Sample.Common.FilterList;
using Sample.Common.UserSessions;
using System.Net;
using ILogger = Serilog.ILogger;

namespace Sample.Api.Controllers
{
    [Route("api/unemployment-insurance")]
    [ApiController]
    public class UnemploymentInsuranceController : HostControllerBaseAnonymous
    {
        public UnemploymentInsuranceController(
           IMediator mediator,
           ILogger logger,
           IUserSession userSession)
           : base(mediator, logger, userSession)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(FilterResult<UnemploymentInsuranceDto>), (int)HttpStatusCode.OK)]
        public virtual async Task<IActionResult> Gets([ModelBinder] FilterRequest dataRequest)
        {
            var result = await _mediator.Send(new GetUnemploymentInsurancesQuery(dataRequest));
            return Ok(result);
        }
    }
}
