using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sample.Common.UserSessions;
using Sample.Common.Web;
using ILogger = Serilog.ILogger;

namespace Sample.Api.Configuration
{
    [AppAuthorize]
    public class HostControllerBase : ControllerBase
    {
        protected readonly IMediator _mediator;
        protected readonly ILogger _logger;
        protected readonly IUserSession _userSession;

        public HostControllerBase(IMediator mediator, ILogger logger, IUserSession userSession)
        {
            _mediator = mediator;
            _logger = logger;
            _userSession = userSession;
        }
    }
}
