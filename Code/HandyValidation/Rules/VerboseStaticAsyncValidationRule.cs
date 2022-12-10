using System;
using System.Threading;
using System.Threading.Tasks;

namespace HandyValidation.Rules
{
    /// <summary>
    /// Wrapper for an asynchronous validation rule, replacing its result with validation message
    /// </summary>
    /// <typeparam name="T">Type of the value</typeparam>
    public class VerboseStaticAsyncValidationRule<T> : IAsyncValidationRule<T, string>
    {
        /// <summary>
        /// Validation rule
        /// </summary>
        protected IAsyncValidationRule<T, object> _rule;

        /// <summary>
        /// Validation message
        /// </summary>
        protected string _message;

        /// <summary>
        /// Source validation rule
        /// </summary>
        public IAsyncValidationRule<T, object> SourceRule
        {
            get
            {
                return _rule;
            }
        }

        /// <summary>
        /// Creates new instance of VerboseStaticAsyncValidationRule
        /// </summary>
        /// <param name="rule">Source rule</param>
        /// <param name="message">Validation message</param>
        public VerboseStaticAsyncValidationRule(IAsyncValidationRule<T, object> rule, string message)
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
        public virtual async Task<string> Validate(T value, CancellationToken cancellationToken = default)
        {
            var sourceRuleValidationResult = await _rule.Validate(value, cancellationToken);

            return sourceRuleValidationResult == null ? null : _message;
        }
    }
}
