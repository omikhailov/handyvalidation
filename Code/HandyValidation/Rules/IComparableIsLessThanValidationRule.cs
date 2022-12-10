using System;
using System.Threading;

namespace HandyValidation.Rules
{
    /// <summary>
    /// Validation rule checking that value is less than specified value
    /// </summary>
    /// <typeparam name="T">Type of the value</typeparam>
    public class IComparableIsLessThanValidationRule<T> : IValidationRule<T, object> where T : IComparable<T>
    {
        private readonly T _border;

        /// <summary>
        /// Create new instance of IComparableIsLessThanValidationRule
        /// </summary>
        /// <param name="border">Value to compare</param>
        public IComparableIsLessThanValidationRule(T border)
        {
            _border = border;
        }

        /// <summary>
        /// Validates value
        /// </summary>
        /// <param name="value">Value to validate</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>String.Empty if validation failed, and null if there are no validation errors</returns>
        public virtual object Validate(T value, CancellationToken cancellationToken = default)
        {
            if (value != null && value.CompareTo(_border) >= 0) return string.Empty;

            return null;   
        }
    }
}