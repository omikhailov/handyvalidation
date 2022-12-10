using System.Threading;

namespace HandyValidation.Rules
{
    /// <summary>
    /// Validation rule checking that string length is in the specified range
    /// </summary>
    public class StringLengthIsInRangeValidationRule : IValidationRule<string, object>
    {
        private readonly int _leastAlowedLength;

        private readonly int _greatrestAllowedLength;

        /// <summary>
        /// Creates new instance of StringLengthIsInRangeValidationRule
        /// </summary>
        /// <param name="leastAlowedLength">Minimum allowed length</param>
        /// <param name="greatrestAllowedLength">Maximum allowed length</param>
        public StringLengthIsInRangeValidationRule(int leastAlowedLength, int greatrestAllowedLength)
        {
            _leastAlowedLength = leastAlowedLength;

            _greatrestAllowedLength = greatrestAllowedLength;
        }

        /// <summary>
        /// Validates value
        /// </summary>
        /// <param name="value">Value to validate</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>String.Empty if validation failed, and null if there are no validation errors</returns>
        public virtual object Validate(string value, CancellationToken cancellationToken = default)
        {
            if (value != null && (value.Length < _leastAlowedLength || value.Length > _greatrestAllowedLength)) return string.Empty;

            return null;   
        }
    }
}