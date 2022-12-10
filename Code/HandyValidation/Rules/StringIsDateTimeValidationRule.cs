using System;
using System.Globalization;
using System.Threading;

namespace HandyValidation.Rules
{
    /// <summary>
    /// Validation rule checking that string represents DateTime value
    /// </summary>
    public class StringIsDateTimeValidationRule : IValidationRule<string, object>
    {
        private readonly DateTimeStyles _dateTimeStyles;

        private readonly IFormatProvider _formatProvider;

        /// <summary>
        /// Creates new instance of StringIsDateTimeValidationRule
        /// </summary>
        /// <param name="dateTimeStyles">DateTimeStyles for parsing</param>
        /// <param name="formatProvider">IFormatProvider for parsing</param>
        public StringIsDateTimeValidationRule(DateTimeStyles dateTimeStyles = DateTimeStyles.None, IFormatProvider formatProvider = null)
        {
            _dateTimeStyles = dateTimeStyles;

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

            return DateTime.TryParse(value, _formatProvider, _dateTimeStyles, out _) ? null : string.Empty;
        }
    }
}