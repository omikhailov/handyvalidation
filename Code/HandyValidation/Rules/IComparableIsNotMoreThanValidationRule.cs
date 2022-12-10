using System;
using System.Threading;

namespace HandyValidation.Rules
{
    /// <summary>
    /// Validation rule checking that value is not greater than specified value
    /// </summary>
    /// <typeparam name="T">Type of the value</typeparam>
    public class IComparableIsNotMoreThanValidationRule<T> : IValidationRule<T, object> where T : IComparable<T>
    {
        private readonly T _greatestAllowedValue;

        /// <summary>
        /// Creates new instance of IComparableIsNotMoreThanValidationRule
        /// </summary>
        /// <param name="greatestAllowedValue">Largest allowed value</param>
        public IComparableIsNotMoreThanValidationRule(T greatestAllowedValue)
        {
            _greatestAllowedValue = greatestAllowedValue;
        }

        /// <summary>
        /// Validates value
        /// </summary>
        /// <param name="value">Value to validate</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>String.Empty if validation failed, and null if there are no validation errors</returns>
        public virtual object Validate(T value, CancellationToken cancellationToken = default)
        {
            if (value != null && value.CompareTo(_greatestAllowedValue) > 0) return string.Empty;

            return null;   
        }
    }
}