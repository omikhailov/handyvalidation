using System;
using System.Globalization;
using System.Threading;

namespace HandyValidation.Rules
{
    public class StringIsDecimalValidationRule : IValidationRule<string, object>
    {
        private readonly NumberStyles _numberStyles;

        private readonly IFormatProvider _formatProvider;

        public StringIsDecimalValidationRule(NumberStyles numberStyles = NumberStyles.Any, IFormatProvider formatProvider = null)
        {
            _numberStyles = numberStyles;

            _formatProvider = formatProvider;
        }

        public virtual object Validate(string value, CancellationToken cancellationToken = default)
        {
            if (value == null) return null;

            return decimal.TryParse(value, _numberStyles, _formatProvider, out _) ? null : string.Empty;
        }
    }
}