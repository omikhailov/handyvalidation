using System;
using System.Globalization;
using System.Threading;

namespace HandyValidation.Rules
{
    public class StringIsDateTimeValidationRule : IValidationRule<string, object>
    {
        private readonly DateTimeStyles _dateTimeStyles;

        private readonly IFormatProvider _formatProvider;

        public StringIsDateTimeValidationRule(DateTimeStyles dateTimeStyles = DateTimeStyles.None, IFormatProvider formatProvider = null)
        {
            _dateTimeStyles = dateTimeStyles;

            _formatProvider = formatProvider;
        }

        public virtual object Validate(string value, CancellationToken cancellationToken = default)
        {
            if (value == null) return null;

            return DateTime.TryParse(value, _formatProvider, _dateTimeStyles, out _) ? null : string.Empty;
        }
    }
}