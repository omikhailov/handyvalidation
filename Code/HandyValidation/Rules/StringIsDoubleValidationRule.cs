using System;
using System.Threading;

namespace HandyValidation.Rules
{
    public class StringIsDoubleValidationRule : IValidationRule<string, object>
    {
        public virtual object Validate(string value, CancellationToken cancellationToken = default)
        {
            if (value == null) return null;

            return double.TryParse(value, out _) ? null : string.Empty;
        }
    }
}