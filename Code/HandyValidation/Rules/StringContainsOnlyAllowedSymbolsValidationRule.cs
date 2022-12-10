using System.Threading;

namespace HandyValidation.Rules
{
    /// <summary>
    /// Validation rule checking that string contains only allowed symbols
    /// </summary>
    public class StringContainsOnlyAllowedSymbolsValidationRule : IValidationRule<string, object>
    {
        private readonly string _allowedSymbols;

        /// <summary>
        /// Creates new instance of StringContainsOnlyAllowedSymbolsValidationRule
        /// </summary>
        /// <param name="allowedSymbols">Allowed symbols</param>
        public StringContainsOnlyAllowedSymbolsValidationRule(string allowedSymbols)
        {
            _allowedSymbols = allowedSymbols;
        }

        /// <summary>
        /// Validates value
        /// </summary>
        /// <param name="value">Value to validate</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>String.Empty if validation failed, and null if there are no validation errors</returns>
        public virtual object Validate(string value, CancellationToken cancellationToken = default)
        {
            if (value == null) return null;
            
            foreach (var c in value) if (_allowedSymbols.IndexOf(c) < 0) return string.Empty;

            return null;
        }
    }
}