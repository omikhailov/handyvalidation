using System.Threading;

namespace HandyValidation.Rules
{
    /// <summary>
    /// Validation rule checking that string contains only digits and letters
    /// </summary>
    public class StringContainsOnlyDigitsOrLettersValidationRule : IValidationRule<string, object>
    {
        /// <summary>
        /// Validates value
        /// </summary>
        /// <param name="value">Value to validate</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>String.Empty if validation failed, and null if there are no validation errors</returns>
        public virtual object Validate(string value, CancellationToken cancellationToken = default)
        {
            if (value == null) return null;

            foreach (var c in value) if (!char.IsLetterOrDigit(c)) return string.Empty;

            return null;
        }
    }
}