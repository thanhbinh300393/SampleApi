using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sample.Api.Configuration;
using Sample.Application.System.Users.CreateUser;
using Sample.Application.System.Users.Delete;
using Sample.Application.System.Users.Dto;
using Sample.Application.System.Users.Gets;
using Sample.Application.System.Users.GetUserDetails;
using Sample.Application.System.Users.UpdateUser;
using Sample.Common.FilterList;
using Sample.Common.UserSessions;
using System.Net;
using ILogger = Serilog.ILogger;

namespace Sample.Api.Controllers.System.User
{
    [Route("api/user")]
    [ApiController]
    public class UserController : HostControllerBase
    {
        public UserController(IMediator mediator, ILogger logger, IUserSession userSession) : base(mediator, logger, userSession)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(FilterResult<UserDto>), (int)HttpStatusCode.OK)]
        public virtual async Task<IActionResult> Gets([ModelBinder] FilterRequest dataRequest)
        {
            var result = await _mediator.Send(new GetUsersQuery(dataRequest));
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
        public virtual async Task<IActionResult> GetDetail(string id)
        {
            var result = (await _mediator.Send(new GetUserDetailsQuery(id)));
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            var result = await _mediator.Send(new CreateUserCommand(request));
            if (request == null)
                return NoContent();
            return Created("", result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateUserRequest request)
        {
            var result = await _mediator.Send(new UpdateUserCommand(request, id));
            if (result == null)
                return NoContent();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _mediator.Send(new DeleteUserCommand(id));
            if (result == null)
                return NoContent();
            return Ok(result);
        }


    }
}
