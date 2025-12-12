using FluentValidation;
using MediatR;
using System.Text;

namespace Sample.Common.CQRS.Commands.Validation
{
    public class CommandValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public CommandValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            {
                var failures = (await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(request, cancellationToken))
                ))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

                if (failures.Any())
                {
                    var errorMessage = "Invalid command, reason: " + Environment.NewLine +
                                       string.Join(Environment.NewLine, failures.Select(f => f.ErrorMessage));

                    throw new InvalidCommandException(errorMessage);
                }

                return await next();
            }
        }
    }
}
