using System;
using System.Collections.Generic;
using System.Threading;

namespace HandyValidation.Rules
{
    /// <summary>
    /// Validation rule checking that string has allowed postfix
    /// </summary>
    public class StringEndsWithValidationRule : IValidationRule<string, object>
    {
        private readonly StringComparison _stringComparisonType;

        private readonly IEnumerable<string> _prefixes;

        /// <summary>
        /// Create new instance of StringEndsWithValidationRule
        /// </summary>
        /// <param name="stringComparisonType">String comparison type</param>
        /// <param name="postfixes">Allowed postfixes</param>
        public StringEndsWithValidationRule(StringComparison stringComparisonType, IEnumerable<string> postfixes)
        {
            _stringComparisonType = stringComparisonType;

            _prefixes = postfixes;
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

            foreach (var prefix in _prefixes)
            {
                if (value.EndsWith(prefix, _stringComparisonType)) return null;
            }

            return string.Empty;
        }
    }
}