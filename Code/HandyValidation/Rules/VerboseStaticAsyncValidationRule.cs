using System;
using System.Threading;
using System.Threading.Tasks;

namespace HandyValidation.Rules
{
    public class VerboseStaticAsyncValidationRule<T> : IAsyncValidationRule<T, string>
    {
        protected IAsyncValidationRule<T, object> _rule;

        protected string _message;

        public IAsyncValidationRule<T, object> SourceRule
        {
            get
            {
                return _rule;
            }
        }

        public VerboseStaticAsyncValidationRule(IAsyncValidationRule<T, object> rule, string message)
        {
            _rule = rule;

            _message = message;
        }

        public virtual async Task<string> Validate(T value, CancellationToken cancellationToken = default)
        {
            var sourceRuleValidationResult = await _rule.Validate(value, cancellationToken);

            return sourceRuleValidationResult == null ? null : _message;
        }
    }
}
