using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sample.Api.Configuration;
using Sample.Application.Geos.Districts.GetDistrictDetails;
using Sample.Application.Geos.Districts.GetDistricts;
using Sample.Common.FilterList;
using Sample.Common.UserSessions;
using Sample.Domain.Geos.Districts;
using System.Net;
using ILogger = Serilog.ILogger;

namespace Sample.Api.Controllers.Geos.Districts
{
    [Route("api/districts")]
    [ApiController]
    public class DistrictController : HostControllerBase
    {
        public DistrictController(IMediator mediator, ILogger logger, IUserSession userSession) : base(mediator, logger, userSession)
        {
        }

        [ResponseCache(VaryByHeader = "User-Agent", VaryByQueryKeys = new[] { "*" }, Duration = int.MaxValue)]
        [HttpGet]
        [ProducesResponseType(typeof(FilterResult<District>), (int)HttpStatusCode.OK)]
        public virtual async Task<IActionResult> Gets([ModelBinder] FilterRequest dataRequest)
        {
            var result = (await _mediator.Send(new GetDistrictQuery(dataRequest)));
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(District), (int)HttpStatusCode.OK)]
        public virtual async Task<IActionResult> GetByID(string id)
        {
            var result = (await _mediator.Send(new GetDistrictDetailsQuery(id)));
            if (result == null)
                return NoContent();
            return Ok(result);
        }
    }
}
