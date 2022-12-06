using System;
using System.Threading;

namespace HandyValidation.Rules
{
    public class StringIsUriValidationRule : IValidationRule<string, object>
    {
        private readonly UriKind _uriKind;

        public StringIsUriValidationRule(UriKind uriKind = UriKind.Absolute)
        {
            _uriKind = uriKind;
        }

        public virtual object Validate(string value, CancellationToken cancellationToken = default)
        {
            if (value == null) return null;

            return Uri.TryCreate(value, _uriKind, out _) ? null : string.Empty;
        }
    }
}