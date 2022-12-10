using System;
using System.Threading;

namespace HandyValidation.Rules
{
    /// <summary>
    /// Validation rule checking that string represents an URI
    /// </summary>
    public class StringIsUriValidationRule : IValidationRule<string, object>
    {
        private readonly UriKind _uriKind;

        /// <summary>
        /// Creates new instance of StringIsUriValidationRule
        /// </summary>
        /// <param name="uriKind">UriKind for parsing</param>
        public StringIsUriValidationRule(UriKind uriKind = UriKind.Absolute)
        {
            _uriKind = uriKind;
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

            return Uri.TryCreate(value, _uriKind, out _) ? null : string.Empty;
        }
    }
}