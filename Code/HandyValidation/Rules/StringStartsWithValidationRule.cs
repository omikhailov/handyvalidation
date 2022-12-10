using System;
using System.Collections.Generic;
using System.Threading;

namespace HandyValidation.Rules
{
    /// <summary>
    /// Validation rule to check that string has allowed prefix
    /// </summary>
    public class StringStartsWithValidationRule : IValidationRule<string, object>
    {
        private readonly StringComparison _stringComparisonType;

        private readonly IEnumerable<string> _prefixes;

        /// <summary>
        /// Creates new instnace of StringStartsWithValidationRule
        /// </summary>
        /// <param name="stringComparisonType">String comparison type</param>
        /// <param name="prefixes">Allowed prefixes</param>
        public StringStartsWithValidationRule(StringComparison stringComparisonType, IEnumerable<string> prefixes)
        {
            _stringComparisonType = stringComparisonType;

            _prefixes = prefixes;
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
                if (value.StartsWith(prefix, _stringComparisonType)) return null;
            }

            return string.Empty;
        }
    }
}