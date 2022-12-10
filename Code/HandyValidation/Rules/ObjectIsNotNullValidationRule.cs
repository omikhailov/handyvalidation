using System;
using System.Threading;

namespace HandyValidation.Rules
{
    /// <summary>
    /// Validation rule checking that value is not null
    /// </summary>
    public class ObjectIsNotNullValidationRule : IValidationRule<object, object>
    {
        /// <summary>
        /// Validates value
        /// </summary>
        /// <param name="value">Value to validate</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>String.Empty if validation failed, and null if there are no validation errors</returns>
        public virtual object Validate(object value, CancellationToken cancellationToken = default)
        {
            if (value == null) return string.Empty;

            return null;
        }
    }
}