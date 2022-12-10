﻿using System.Threading;

namespace HandyValidation.Rules
{
    /// <summary>
    /// Validation rule checking that string has minimum allowed length
    /// </summary>
    public class StringMinLengthValidationRule : IValidationRule<string, object>
    {
        private readonly int _symbols;

        /// <summary>
        /// Creates new instance of StringMinLengthValidationRule
        /// </summary>
        /// <param name="symbols">Minimum allowed length</param>
        public StringMinLengthValidationRule(int symbols)
        {
            _symbols = symbols;
        }

        /// <summary>
        /// Validates value
        /// </summary>
        /// <param name="value">Value to validate</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>String.Empty if validation failed, and null if there are no validation errors</returns>
        public virtual object Validate(string value, CancellationToken cancellationToken = default)
        {
            if (value != null && value.Length < _symbols) return string.Empty;

            return null;   
        }
    }
}