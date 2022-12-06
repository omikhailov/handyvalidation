using System.Threading;

namespace HandyValidation.Rules
{
    public class StringMinLengthValidationRule : IValidationRule<string, object>
    {
        private readonly int _symbols;

        public StringMinLengthValidationRule(int symbols)
        {
            _symbols = symbols;
        }

        public virtual object Validate(string value, CancellationToken cancellationToken = default)
        {
            if (value != null && value.Length < _symbols) return string.Empty;

            return null;   
        }
    }
}