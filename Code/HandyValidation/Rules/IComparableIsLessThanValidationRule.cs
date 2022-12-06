using System;
using System.Threading;

namespace HandyValidation.Rules
{
    public class IComparableIsLessThanValidationRule<T> : IValidationRule<T, object> where T : IComparable<T>
    {
        private readonly T _border;

        public IComparableIsLessThanValidationRule(T border)
        {
            _border = border;
        }

        public virtual object Validate(T value, CancellationToken cancellationToken = default)
        {
            if (value != null && value.CompareTo(_border) >= 0) return string.Empty;

            return null;   
        }
    }
}