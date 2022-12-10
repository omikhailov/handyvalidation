using System;
using System.Globalization;
using System.Threading;

namespace HandyValidation.Rules
{
    /// <summary>
    /// Validation rule checking that string represents decimal vlaue
    /// </summary>
    public class StringIsDecimalValidationRule : IValidationRule<string, object>
    {
        private readonly NumberStyles _numberStyles;

        private readonly IFormatProvider _formatProvider;

        /// <summary>
        /// Creates new instance of StringIsDecimalValidationRule
        /// </summary>
        /// <param name="numberStyles">NumberStyles for parsing</param>
        /// <param name="formatProvider">IFormatProvider for parsing</param>
        public StringIsDecimalValidationRule(NumberStyles numberStyles = NumberStyles.Any, IFormatProvider formatProvider = null)
        {
            _numberStyles = numberStyles;

            _formatProvider = formatProvider;
        }

        /// <summary>
        /// Validates value
        /// </summary>
        /// <param name="value">Value to validate</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>String.Empty if validation failed, and null if there are no validation errors</returns>
        public virtual object Validate(string value, CancellationToken cancellationToken = default)
        {
            if (value == null) return null;

            return decimal.TryParse(value, _numberStyles, _formatProvider, out _) ? null : string.Empty;
        }
    }
}