using System;
using System.Threading;

namespace HandyValidation.Rules
{
    public class StringIsNotNullOrEmptyValidationRule : IValidationRule<string, object>
    {
        public virtual object Validate(string value, CancellationToken cancellationToken = default)
        {
            return string.IsNullOrEmpty(value) ? string.Empty : null;
        }
    }
}