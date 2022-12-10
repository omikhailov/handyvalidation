using System.Text.RegularExpressions;
using System.Threading;

namespace HandyValidation.Rules
{
    /// <summary>
    /// Validation rule checking that string matches specified regular expression
    /// </summary>
    public class RegularExpressionValidationRule : IValidationRule<string, object>
    {
        private readonly string _expression;

        private readonly RegexOptions _options;

        /// <summary>
        /// Create new instance of RegularExpressionValidationRule
        /// </summary>
        /// <param name="expression">Regular expression</param>
        /// <param name="options">RegEx options</param>
        public RegularExpressionValidationRule(string expression, RegexOptions options = RegexOptions.None)
        {
            _expression = expression;

            _options = options;
        }

        /// <summary>
        /// Validates value
        /// </summary>
        /// <param name="value">Value to validate</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>String.Empty if validation failed, and null if there are no validation errors</returns>
        public virtual object Validate(string value, CancellationToken cancellationToken = default)
        {
            if (value == null || _expression == null) return null;

            return Regex.IsMatch(value, _expression, _options);
        }
    }
}