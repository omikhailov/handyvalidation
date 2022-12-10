using System;
using System.Threading;

namespace HandyValidation.Rules
{
    /// <summary>
    /// Validation rule checking that custom condition is true
    /// </summary>
    /// <typeparam name="T">Type of the value</typeparam>
    public class CustomValidationRule<T> : IValidationRule<T, object>
    {
        private readonly Func<T, object> _function;

        /// <summary>
        /// Creartes new instance of CustomValidationRule
        /// </summary>
        /// <param name="function">Validation function</param>
        public CustomValidationRule(Func<T, object> function)
        {
            _function = function;
        }

        /// <summary>
        /// Validates value
        /// </summary>
        /// <param name="value">Value to validate</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>String.Empty if validation failed, and null if there are no validation errors</returns>
        public object Validate(T value, CancellationToken cancellationToken = default)
        {
            return _function(value);
        }
    }
}