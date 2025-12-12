using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sample.Api.Configuration;
using Sample.Application.Geos.Wards.GetWardDetails;
using Sample.Application.Geos.Wards.GetWards;
using Sample.Common.FilterList;
using Sample.Common.UserSessions;
using Sample.Domain.Geos.Wards;
using System.Net;
using ILogger = Serilog.ILogger;

namespace Sample.Api.Controllers.Geos.Wards
{
    [Route("api/wards")]
    [ApiController]
    public class WardController : HostControllerBase
    {
        public WardController(IMediator mediator, ILogger logger, IUserSession userSession) : base(mediator, logger, userSession)
        {
        }

        [ResponseCache(VaryByHeader = "User-Agent", VaryByQueryKeys = new[] { "*" }, Duration = int.MaxValue)]
        [HttpGet]
        [ProducesResponseType(typeof(FilterResult<Ward>), (int)HttpStatusCode.OK)]
        public virtual async Task<IActionResult> Gets([ModelBinder] FilterRequest dataRequest)
        {
            var result = (await _mediator.Send(new GetWardQuery(dataRequest)));
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Ward), (int)HttpStatusCode.OK)]
        public virtual async Task<IActionResult> GetByID(string id)
        {
            var result = (await _mediator.Send(new GetWardDetailsQuery(id)));
            if (result == null)
                return NoContent();
            return Ok(result);
        }
    }
}