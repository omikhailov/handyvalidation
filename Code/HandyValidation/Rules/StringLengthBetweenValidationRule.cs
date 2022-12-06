using System.Threading;

namespace HandyValidation.Rules
{
    public class StringLengthIsInRangeValidationRule : IValidationRule<string, object>
    {
        private readonly int _leastAlowedLength;

        private readonly int _greatrestAllowedLength;

        public StringLengthIsInRangeValidationRule(int leastAlowedLength, int greatrestAllowedLength)
        {
            _leastAlowedLength = leastAlowedLength;

            _greatrestAllowedLength = greatrestAllowedLength;
        }

        public virtual object Validate(string value, CancellationToken cancellationToken = default)
        {
            if (value != null && (value.Length < _leastAlowedLength || value.Length > _greatrestAllowedLength)) return string.Empty;

            return null;   
        }
    }
}