using System;

namespace Sample.Common.CQRS.Commands.Validation
{
    public class InvalidCommandException : Exception
    {
        public string Details { get; }
        public InvalidCommandException(string message, string details) : base(message)
        {
            this.Details = details;
        }

        public InvalidCommandException(string? message) : base(message)
        {
        }
    }
}