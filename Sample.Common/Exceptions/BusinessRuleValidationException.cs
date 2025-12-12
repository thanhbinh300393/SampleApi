using System;
using System.Net;

namespace Sample.Common.Exceptions
{
    public class BusinessRuleValidationException : Exception, IExceptionBase
    {
        public IBusinessRule BrokenRule { get; }


        public object? DataTranfer => null;
        public string UserFriendlyMessage => BrokenRule.Message;

        public HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;

        public BusinessRuleValidationException(IBusinessRule brokenRule) : base(brokenRule.Message)
        {
            BrokenRule = brokenRule;
        }

        public override string ToString()
        {
            return $"{BrokenRule.GetType().FullName}: {BrokenRule.Message}";
        }
    }
}