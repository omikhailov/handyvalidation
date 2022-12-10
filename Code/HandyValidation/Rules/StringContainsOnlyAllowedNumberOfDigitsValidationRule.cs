using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace HandyValidation.Rules
{
    /// <summary>
    /// Validation rule checlking that string contains only allowed count of digits
    /// </summary>
    public class StringContainsOnlyAllowedNumberOfDigitsValidationRule : IValidationRule<string, object>
    {
        private readonly IEnumerable<int> _counts;

        /// <summary>
        /// Create new instance of StringContainsOnlyAllowedNumberOfDigitsValidationRule
        /// </summary>
        /// <param name="counts">Allowed counts of digits</param>
        public StringContainsOnlyAllowedNumberOfDigitsValidationRule(IEnumerable<int> counts)
        {
            _counts = counts;
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

            var count = value.Count(c => char.IsDigit(c));

            if (!_counts.Contains(count)) return string.Empty;

            return null;
        }
    }
}