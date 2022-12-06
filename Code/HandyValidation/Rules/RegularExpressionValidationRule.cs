using System.Text.RegularExpressions;
using System.Threading;

namespace HandyValidation.Rules
{
    public class RegularExpressionValidationRule : IValidationRule<string, object>
    {
        private readonly string _expression;

        private readonly RegexOptions _options;

        public RegularExpressionValidationRule(string expression, RegexOptions options = RegexOptions.None)
        {
            _expression = expression;

            _options = options;
        }

        public virtual object Validate(string value, CancellationToken cancellationToken = default)
        {
            if (value == null || _expression == null) return null;

            return Regex.IsMatch(value, _expression, _options);
        }
    }
}