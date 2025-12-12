using MediatR;
using System;

namespace Sample.Common.CQRS.Commands
{
    public interface ICommand : IRequest
    {
        Guid RequestId { get; }
    }

    public interface ICommand<out TResult> : IRequest<TResult>
    {
        Guid RequestId { get; }
    }
}