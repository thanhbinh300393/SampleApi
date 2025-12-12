using Sample.Common.UserSessions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace Sample.Api.Configuration
{
    public class HostControllerBaseAnonymous : ControllerBase
    {
        protected readonly IMediator _mediator;
        protected readonly ILogger _logger;
        protected readonly IUserSession _userSession;

        public HostControllerBaseAnonymous(IMediator mediator, ILogger logger, IUserSession userSession)
        {
            _mediator = mediator;
            _logger = logger;
            _userSession = userSession;
        }
    }
}
