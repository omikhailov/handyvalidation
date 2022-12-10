using System;
using System.Threading;

namespace HandyValidation.Rules
{
    /// <summary>
    /// Validation rule checking that value matches one of the specified values
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    public class IEquatableAllowedValuesValidationRule<T> : IValidationRule<T, object> where T : IEquatable<T>
    {
        private readonly T[] _values;

        /// <summary>
        /// Creates new instance of IEquatableAllowedValuesValidationRule
        /// </summary>
        /// <param name="values">Allowed values</param>
        public IEquatableAllowedValuesValidationRule(T[] values)
        {
            _values = values;
        }

        /// <summary>
        /// Validates value
        /// </summary>
        /// <param name="value">Value to validate</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>String.Empty if validation failed, and null if there are no validation errors</returns>
        public virtual object Validate(T value, CancellationToken cancellationToken = default)
        {
            foreach (var allowed in _values) if (value.Equals(allowed)) return null;

            return string.Empty;
        }
    }
}