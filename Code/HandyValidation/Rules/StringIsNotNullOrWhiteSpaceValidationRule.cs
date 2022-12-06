using System.Threading;

namespace HandyValidation.Rules
{
    public class StringIsNotNullOrWhiteSpaceValidationRule : IValidationRule<string, object>
    {
        public virtual object Validate(string value, CancellationToken cancellationToken = default)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : null;
        }
    }
}