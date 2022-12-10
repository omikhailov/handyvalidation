using System;
using System.Threading;

namespace HandyValidation.Rules
{
    /// <summary>
    /// Validation rule checking that value is not less than specified value
    /// </summary>
    /// <typeparam name="T">Type of the value</typeparam>
    public class IComparableIsAtLeastValidationRule<T> : IValidationRule<T, object> where T : IComparable<T>
    {
        private readonly T _leastAllowedValue;

        /// <summary>
        /// Creates new instance of IComparableIsAtLeastValidationRule
        /// </summary>
        /// <param name="leastAllowedValue">Value to compare</param>
        public IComparableIsAtLeastValidationRule(T leastAllowedValue)
        {
            _leastAllowedValue = leastAllowedValue;
        }

        /// <summary>
        /// Validates value
        /// </summary>
        /// <param name="value">Value to validate</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>String.Empty if validation failed, and null if there are no validation errors</returns>
        public virtual object Validate(T value, CancellationToken cancellationToken = default)
        {
            if (value != null && value.CompareTo(_leastAllowedValue) < 0) return string.Empty;

            return null;   
        }
    }
}