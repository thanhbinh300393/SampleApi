using MediatR;
using System;

namespace Sample.Common.CQRS.Queries
{
    public abstract class QueryBase<TResult> : IRequest<TResult>
    {
        public Guid RequestId { get; set; }

        protected QueryBase()
        {
            this.RequestId = Guid.NewGuid();
        }

        protected QueryBase(Guid requestId)
        {
            this.RequestId = requestId;
        }
    }
}