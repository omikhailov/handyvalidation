using System;
using System.Threading;
using System.Threading.Tasks;

namespace HandyValidation.Rules
{
    public class VerboseDynamicAsyncValidationRule<T> : IAsyncValidationRule<T, string>
    {
        protected IAsyncValidationRule<T, object> _sourceRule;

        protected Func<T, string> _messageFunction;

        public IAsyncValidationRule<T, object> SourceRule 
        { 
            get 
            { 
                return _sourceRule; 
            } 
        }

        public VerboseDynamicAsyncValidationRule(IAsyncValidationRule<T, object> rule, Func<T, string> messageFunction)
        {
            _sourceRule = rule;

            _messageFunction = messageFunction;
        }

        public virtual async Task<string> Validate(T value, CancellationToken cancellationToken = default)
        {
            var sourceRuleValidationResult = await _sourceRule.Validate(value, cancellationToken);

            return sourceRuleValidationResult == null ? null : _messageFunction(value);
        }
    }
}
