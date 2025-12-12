using MediatR;

namespace Sample.Common.CQRS.Queries
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}