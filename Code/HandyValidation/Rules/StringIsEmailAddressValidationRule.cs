using System.Net.Mail;
using System.Threading;

namespace HandyValidation.Rules
{
    public class StringIsEmailAddressValidationRule : IValidationRule<string, object>
    {
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