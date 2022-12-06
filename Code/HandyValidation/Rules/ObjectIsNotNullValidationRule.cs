using System;
using System.Threading;

namespace HandyValidation.Rules
{
    public class ObjectIsNotNullValidationRule : IValidationRule<object, object>
    {
        public virtual object Validate(object value, CancellationToken cancellationToken = default)
        {
            if (value == null) return string.Empty;

            return null;
        }
    }
}