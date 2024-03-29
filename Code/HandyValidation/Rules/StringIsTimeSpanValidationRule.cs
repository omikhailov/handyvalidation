﻿using System;
using System.Threading;

namespace HandyValidation.Rules
{
    /// <summary>
    /// Validation rule checking that string represents TimeSpan
    /// </summary>
    public class StringIsTimeSpanValidationRule : IValidationRule<string, object>
    {
        private readonly IFormatProvider _formatProvider;

        /// <summary>
        /// Creates new instance of StringIsTimeSpanValidationRule
        /// </summary>
        /// <param name="formatProvider">IFormatProvider for parsing</param>
        public StringIsTimeSpanValidationRule(IFormatProvider formatProvider = null)
        {
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

            return TimeSpan.TryParse(value, _formatProvider, out _) ? null : string.Empty;
        }
    }
}