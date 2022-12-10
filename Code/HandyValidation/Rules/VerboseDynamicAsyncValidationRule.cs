using System;
using System.Threading;
using System.Threading.Tasks;

namespace HandyValidation.Rules
{
    /// <summary>
    /// Wrapper for an asynchronous validation rule, dynamically replacing its result with validation message
    /// </summary>
    /// <typeparam name="T">Type of the value</typeparam>
    public class VerboseDynamicAsyncValidationRule<T> : IAsyncValidationRule<T, string>
    {
        /// <summary>
        /// Validation rule
        /// </summary>
        protected IAsyncValidationRule<T, object> _sourceRule;

        /// <summary>
        /// Validation message function
        /// </summary>
        protected Func<T, string> _messageFunction;

        /// <summary>
        /// Source validation rule
        /// </summary>
        public IAsyncValidationRule<T, object> SourceRule 
        { 
            get 
            { 
                return _sourceRule; 
            } 
        }

        /// <summary>
        /// Creates new instance of VerboseDynamicAsyncValidationRule
        /// </summary>
        /// <param name="rule">Source rule</param>
        /// <param name="messageFunction">Validation message function</param>
        public VerboseDynamicAsyncValidationRule(IAsyncValidationRule<T, object> rule, Func<T, string> messageFunction)
        {
            _sourceRule = rule;

            _messageFunction = messageFunction;
        }

        /// <summary>
        /// Validates value
        /// </summary>
        /// <param name="value">Value to validate</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>String.Empty if validation failed, and null if there are no validation errors</returns>
        public virtual async Task<string> Validate(T value, CancellationToken cancellationToken = default)
        {
            var sourceRuleValidationResult = await _sourceRule.Validate(value, cancellationToken);

            return sourceRuleValidationResult == null ? null : _messageFunction(value);
        }
    }
}
