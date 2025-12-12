using Sample.Common.UserSessions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.Common.CQRS.Queries
{
    public abstract class QueryHandlerBase<TQuery, TResult> : IRequestHandler<TQuery, TResult> where TQuery : QueryBase<TResult>
    {
        private readonly IServiceProvider _serviceProvider;

        protected IMediator _mediator => (IMediator)_serviceProvider.GetService(typeof(IMediator));
        protected ILogger _logger => (ILogger)_serviceProvider.GetService(typeof(ILogger));
        protected IUserSession _userSession => (IUserSession)_serviceProvider.GetService(typeof(IUserSession));
        protected IConfiguration _configuration => (IConfiguration)_serviceProvider.GetService(typeof(IConfiguration));

        private CancellationToken _cancellationToken;

        public QueryHandlerBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task<TResult> Handle(TQuery request, CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            return this.QueryHandle(request, _cancellationToken);
        }

        public abstract Task<TResult> QueryHandle(TQuery request, CancellationToken cancellationToken);
    }
}
