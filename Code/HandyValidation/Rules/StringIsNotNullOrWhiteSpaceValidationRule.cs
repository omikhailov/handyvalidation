using System.Threading;

namespace HandyValidation.Rules
{
    /// <summary>
    /// Validation rule checking that sting is not null or white space
    /// </summary>
    public class StringIsNotNullOrWhiteSpaceValidationRule : IValidationRule<string, object>
    {
        /// <summary>
        /// Validates value
        /// </summary>
        /// <param name="value">Value to validate</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>String.Empty if validation failed, and null if there are no validation errors</returns>
        public virtual object Validate(string value, CancellationToken cancellationToken = default)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : null;
        }
    }
}