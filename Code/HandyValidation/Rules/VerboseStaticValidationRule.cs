using System;
using System.Threading;

namespace HandyValidation.Rules
{
    /// <summary>
    /// Wrapper for a validation rule, replacing its result with validation message
    /// </summary>
    /// <typeparam name="T">Type of the value</typeparam>
    public class VerboseStaticValidationRule<T> : IValidationRule<T, string>
    {
        /// <summary>
        /// Validation rule
        /// </summary>
        protected IValidationRule<T, object> _rule;

        /// <summary>
        /// Validation message
        /// </summary>
        protected string _message;

        /// <summary>
        /// Source validation rule
        /// </summary>
        public IValidationRule<T, object> SourceRule
        {
            get
            {
                return _rule;
            }
        }

        /// <summary>
        /// Creates new instance of VerboseStaticValidationRule
        /// </summary>
        /// <param name="rule">Source rule</param>
        /// <param name="message">Validation message</param>
        public VerboseStaticValidationRule(IValidationRule<T, object> rule, string message)
        {
            _rule = rule;

            _message = message;
        }

        /// <summary>
        /// Validates value
        /// </summary>
        /// <param name="value">Value to validate</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>String.Empty if validation failed, and null if there are no validation errors</returns>
        public virtual string Validate(T value, CancellationToken cancellationToken = default)
        {
            return _rule.Validate(value, cancellationToken) == null ? null : _message;
        }
    }
}
