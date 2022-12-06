using System;
using System.Threading;

namespace HandyValidation.Rules
{
    public class VerboseDynamicValidationRule<T> : IValidationRule<T, string>
    {
        protected IValidationRule<T, object> _sourceRule;

        protected Func<T, string> _messageFunction;

        public IValidationRule<T, object> SourceRule 
        { 
            get 
            { 
                return _sourceRule; 
            } 
        }

        public VerboseDynamicValidationRule(IValidationRule<T, object> rule, Func<T, string> messageFunction)
        {
            _sourceRule = rule;

            _messageFunction = messageFunction;
        }

        public virtual string Validate(T value, CancellationToken cancellationToken = default)
        {
            return _sourceRule.Validate(value, cancellationToken) == null ? null : _messageFunction(value);
        }
    }
}
