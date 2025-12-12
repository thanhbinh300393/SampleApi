using Sample.Common.FilterList;
using Sample.Common.UserSessions;
using Sample.Api.Configuration;
using Sample.Application.Collective.Positions.Create;
using Sample.Application.Collective.Positions.Delete;
using Sample.Application.Collective.Positions.Dto;
using Sample.Application.Collective.Positions.GetDetail;
using Sample.Application.Collective.Positions.Gets;
using Sample.Application.Collective.Positions.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ILogger = Serilog.ILogger;

namespace Sample.Api.Controllers.Collective.Positions
{
    [Route("api/position")]
    public class PositionController : HostControllerBase
    {
        public PositionController(IMediator mediator, ILogger logger, IUserSession userSession) : base(mediator, logger, userSession)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(FilterResult<PositionDto>), (int)HttpStatusCode.OK)]
        public virtual async Task<IActionResult> Gets([ModelBinder] FilterRequest dataRequest)
        {
            var result = (await _mediator.Send(new GetPositionsQuery(dataRequest)));
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PositionDto), (int)HttpStatusCode.OK)]
        public virtual async Task<IActionResult> GetDetail(long id)
        {
            var result = (await _mediator.Send(new GetPositionDetailQuery(id)));
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PositionDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create([FromBody] CreatePositionRequest request)
        {
            var result = await _mediator.Send(new CreatePositionCommand(request));
            if (request == null)
                return NoContent();
            return Created("", result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PositionDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update(long id, [FromBody] UpdatePositionRequest request)
        {
            var result = await _mediator.Send(new UpdatePositionCommand(request, id));
            if (result == null)
                return NoContent();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(PositionDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _mediator.Send(new DeletePositionCommand(id));
            if (result == null)
                return NoContent();
            return Ok(result);
        }
    }
}
