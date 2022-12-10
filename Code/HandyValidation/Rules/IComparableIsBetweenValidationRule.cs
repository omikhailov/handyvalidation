using System;
using System.Threading;

namespace HandyValidation.Rules
{
    /// <summary>
    /// Validation rule checking that value is greater and less than specified values
    /// </summary>
    /// <typeparam name="T">Type of the value</typeparam>
    public class IComparableIsBetweenValidationRule<T> : IValidationRule<T, object> where T: IComparable<T>
    {
        private readonly T _left;

        private readonly T _right;

        /// <summary>
        /// Creates new instance
        /// </summary>
        /// <param name="left">Lesser value to compare</param>
        /// <param name="right">Greater value to compare</param>
        public IComparableIsBetweenValidationRule(T left, T right)
        {
            _left = left;

            _right = right;
        }

        /// <summary>
        /// Validates value
        /// </summary>
        /// <param name="value">Value to validate</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>String.Empty if validation failed, and null if there are no validation errors</returns>
        public virtual object Validate(T value, CancellationToken cancellationToken = default)
        {
            if (value != null && (value.CompareTo(_left) <= 0 || value.CompareTo(_right) >= 0)) return string.Empty;

            return null;   
        }
    }
}