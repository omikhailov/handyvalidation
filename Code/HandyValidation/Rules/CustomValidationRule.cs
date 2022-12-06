using System;
using System.Threading;

namespace HandyValidation.Rules
{
    public class CustomValidationRule<T> : IValidationRule<T, object>
    {
        private readonly Func<T, object> _function;

        public CustomValidationRule(Func<T, object> function)
        {
            _function = function;
        }

        public object Validate(T value, CancellationToken cancellationToken = default)
        {
            return _function(value);
        }
    }
}