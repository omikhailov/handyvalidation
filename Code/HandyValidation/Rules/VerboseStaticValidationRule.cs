using System;
using System.Threading;

namespace HandyValidation.Rules
{
    public class VerboseStaticValidationRule<T> : IValidationRule<T, string>
    {
        protected IValidationRule<T, object> _rule;

        protected string _message;

        public IValidationRule<T, object> SourceRule
        {
            get
            {
                return _rule;
            }
        }

        public VerboseStaticValidationRule(IValidationRule<T, object> rule, string message)
        {
            _rule = rule;

            _message = message;
        }

        public virtual string Validate(T value, CancellationToken cancellationToken = default)
        {
            return _rule.Validate(value, cancellationToken) == null ? null : _message;
        }
    }
}
