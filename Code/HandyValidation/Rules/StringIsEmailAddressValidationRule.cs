using System.Net.Mail;
using System.Threading;

namespace HandyValidation.Rules
{
    /// <summary>
    /// Validation rule checking that string represents email address
    /// </summary>
    public class StringIsEmailAddressValidationRule : IValidationRule<string, object>
    {
        /// <summary>
        /// Validates value
        /// </summary>
        /// <param name="value">Value to validate</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>String.Empty if validation failed, and null if there are no validation errors</returns>
        public virtual object Validate(string value, CancellationToken cancellationToken = default)
        {
            if (value == null) return null;

            try
            {
                new MailAddress(value);
            }
            catch
            {
                return string.Empty;
            }

            return null;
        }
    }
}