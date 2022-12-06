using System.Threading;

namespace HandyValidation.Rules
{
    public class StringContainsOnlyAllowedSymbolsValidationRule : IValidationRule<string, object>
    {
        private readonly string _allowedSymbols;

        public StringContainsOnlyAllowedSymbolsValidationRule(string allowedSymbols)
        {
            _allowedSymbols = allowedSymbols;
        }

        public virtual object Validate(string value, CancellationToken cancellationToken = default)
        {
            if (value == null) return null;
            
            foreach (var c in value) if (_allowedSymbols.IndexOf(c) < 0) return string.Empty;

            return null;
        }
    }
}