using System;
using System.Threading;

namespace HandyValidation.Rules
{
    /// <summary>
    /// Wrapper for a validation rule, dynamically replacing its result with validation message
    /// </summary>
    /// <typeparam name="T">Type of the value</typeparam>
    public class VerboseDynamicValidationRule<T> : IValidationRule<T, string>
    {
        /// <summary>
        /// Validation rule
        /// </summary>
        protected IValidationRule<T, object> _sourceRule;

        /// <summary>
        /// Validation message function
        /// </summary>
        protected Func<T, string> _messageFunction;

        /// <summary>
        /// Source validation rule
        /// </summary>
        public IValidationRule<T, object> SourceRule 
        { 
            get 
            { 
                return _sourceRule; 
            } 
        }

        /// <summary>
        /// Creates new instance of VerboseDynamicValidationRule
        /// </summary>
        /// <param name="rule">Source rule</param>
        /// <param name="messageFunction">Validation message function</param>
        public VerboseDynamicValidationRule(IValidationRule<T, object> rule, Func<T, string> messageFunction)
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
        public virtual string Validate(T value, CancellationToken cancellationToken = default)
        {
            return _sourceRule.Validate(value, cancellationToken) == null ? null : _messageFunction(value);
        }
    }
}
