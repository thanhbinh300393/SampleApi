using Sample.Common.FilterList;
using Sample.Common.UserSessions;
using Sample.Application.Geos.Cities.GetCities;
using Sample.Application.Geos.Cities.GetCityDetails;
using Sample.Domain.Geos.Cities;
using Sample.Api.Configuration;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ILogger = Serilog.ILogger;

namespace Sample.Api.Controllers.Geos.Cities
{
    [Route("api/cities")]
    [ApiController]
    public class CityController : HostControllerBaseAnonymous
    {
        public CityController(
            IMediator mediator,
            ILogger logger,
            IUserSession userSession)
            : base(mediator, logger, userSession)
        {
        }

        //[ResponseCache(VaryByHeader = "User-Agent", VaryByQueryKeys = new[] { "*" }, Duration = int.MaxValue)]
        [HttpGet]
        [ProducesResponseType(typeof(FilterResult<City>), (int)HttpStatusCode.OK)]
        public virtual async Task<IActionResult> Gets([ModelBinder] FilterRequest dataRequest)
        {
            var result = await _mediator.Send(new GetCitiesQuery(dataRequest));
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(City), (int)HttpStatusCode.OK)]
        public virtual async Task<IActionResult> GetByID(string id)
        {
            var result = await _mediator.Send(new GetCityDetailsQuery(id));

            if (result == null)
                return NoContent();

            return Ok(result);
        }
    }
}
