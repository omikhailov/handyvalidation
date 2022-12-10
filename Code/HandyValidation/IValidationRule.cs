using System;
using System.Threading;

namespace HandyValidation
{
    /// <summary>
    /// Validation rule
    /// </summary>
    /// <typeparam name="T">Type of the value</typeparam>
    /// <typeparam name="I">Type of the issue</typeparam>
    public interface IValidationRule<in T, out I>
    {
        /// <summary>
        /// Validates value
        /// </summary>
        /// <param name="value">Value to validate</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Validation issue or null</returns>
        I Validate(T value, CancellationToken cancellationToken);
    }
}
