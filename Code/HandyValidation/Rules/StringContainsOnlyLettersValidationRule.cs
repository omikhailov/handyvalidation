using System.Threading;

namespace HandyValidation.Rules
{
    /// <summary>
    /// Validation message checking that string contains only letters
    /// </summary>
    public class StringContainsOnlyLettersValidationRule : IValidationRule<string, object>
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

            foreach (var c in value) if (!char.IsLetter(c)) return string.Empty;

            return null;
        }
    }
}