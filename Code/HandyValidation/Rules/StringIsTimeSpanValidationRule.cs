using System;
using System.Threading;

namespace HandyValidation.Rules
{
    public class StringIsTimeSpanValidationRule : IValidationRule<string, object>
    {
        private readonly IFormatProvider _formatProvider;

        public StringIsTimeSpanValidationRule(IFormatProvider formatProvider = null)
        {
            _formatProvider = formatProvider;
        }

        public virtual object Validate(string value, CancellationToken cancellationToken = default)
        {
            if (value == null) return null;

            return TimeSpan.TryParse(value, _formatProvider, out _) ? null : string.Empty;
        }
    }
}