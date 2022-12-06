using System;
using System.Threading;

namespace HandyValidation.Rules
{
    public class StringIsIntegerValidationRule : IValidationRule<string, object>
    {
        public virtual object Validate(string value, CancellationToken cancellationToken = default)
        {
            if (value == null) return null;

            return int.TryParse(value, out _) ? null : string.Empty;
        }
    }
}