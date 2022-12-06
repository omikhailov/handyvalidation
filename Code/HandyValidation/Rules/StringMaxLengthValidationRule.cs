using System.Threading;

namespace HandyValidation.Rules
{
    public class StringMaxLengthValidationRule : IValidationRule<string, object>
    {
        private readonly int _symbols;

        public StringMaxLengthValidationRule(int symbols)
        {
            _symbols = symbols;
        }

        public virtual object Validate(string value, CancellationToken cancellationToken = default)
        {
            if (value != null && value.Length > _symbols) return string.Empty;

            return null;   
        }
    }
}